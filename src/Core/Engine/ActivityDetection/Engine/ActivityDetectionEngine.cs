using Encoo.ProcessMining.DataContext;
using Encoo.ProcessMining.DataContext.Model;
using Encoo.ProcessMining.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Encoo.ProcessMining.Engine;

public class ActivityDetectionEngine
{
    private readonly ILogger<ActivityDetectionEngine> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly ActivityDetectionEngineOptions options;
    private readonly IKnowledgeBaseDataContext knowledgeBaseContext;
    private readonly IActivityDetectorsManager detectorsManager = new ActivityDetectorsManager();
    private KnowledgeBase knowledgeBase;
    private TaskCompletionSource triggerExecute = new();
    private readonly CancellationTokenSource cancelExecute = new();

    public ActivityDetectionEngine(
        ILogger<ActivityDetectionEngine> logger,
        IOptions<ActivityDetectionEngineOptions> options,
        IServiceProvider serviceProvider,
        IKnowledgeBaseDataContext knowledgeBaseContext)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.options = options.Value;
        this.knowledgeBaseContext = knowledgeBaseContext;
    }

    public void TriggerExecute()
    {
        this.triggerExecute.SetResult();
        this.triggerExecute = new TaskCompletionSource();
    }

    public Task StartAsync(CancellationToken token)
    {
        this.ExecuteAsync(token).FireAndForget();
        return Task.CompletedTask;
    }

#pragma warning disable IDE0060 // Remove unused parameter
    public Task StopAsync(CancellationToken token)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        this.cancelExecute.Cancel();
        return Task.CompletedTask;
    }

    private async Task ExecuteAsync(CancellationToken token)
    {
        token = CancellationTokenSource.CreateLinkedTokenSource(token, this.cancelExecute.Token).Token;
        bool forceReload = true;
        while (!token.IsCancellationRequested)
        {
            var trigger = this.triggerExecute;
            var scope = this.serviceProvider.CreateScope();
            var waitSeconds = 0;

            try
            {
                this.knowledgeBase = await this.knowledgeBaseContext.GetKnowledgeBaseAsync(forceReload, token);
                forceReload = false;

                this.detectorsManager.Initialize(this.knowledgeBase.FlattenedDefinitions);

                var dataRecordDataContext = scope.ServiceProvider.GetRequiredService<IDataRecordDataContext>();

                var records = await dataRecordDataContext.LoadDataRecordToDetectAsync(
                    this.knowledgeBase.Watermark,
                    this.options.BatchLoadingSize,
                    token).ToListAsync(token);

                var workItemSize = records.Count / Environment.ProcessorCount;
                workItemSize = Math.Max(1, workItemSize);
                workItemSize = Math.Min(this.options.MaxWorkItemSize, workItemSize);

                var recordGroups = Enumerable.Range(0, records.Count).GroupBy(i => i / workItemSize, i => records[i]);

                var results = await Task.WhenAll(recordGroups.Select(recordGroup => Task.Run(() => this.Detect(recordGroup))));

                var activityInstanceDataContext = scope.ServiceProvider.GetRequiredService<IActivityInstanceDataContext>();
                await activityInstanceDataContext.SaveActivityInstancesAsync(results.SelectMany(r => r).Where(r => r.ActivityInstance != null).Select(r => r.ActivityInstance), token);

                if (records.Count == 0)
                {
                    waitSeconds = this.options.DetectionIntervalSeconds;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception happened {ex}", ex);
                forceReload = true;
                waitSeconds = this.options.ErrorRetryIntervalSeconds;
            }
            finally
            {
                scope.Dispose();
            }

            await Task.WhenAny(Task.Delay(TimeSpan.FromSeconds(waitSeconds), token), trigger.Task);
        }
    }

    private ActivityDetectionResult Detect(DataRecord record)
    {
        Debug.Assert(!record.IsDeleted, $"Record is deleted {record}");
        Debug.Assert(record.IsTemplateDetected, $"Record is not template detected {record}");
        Debug.Assert(!record.IsActivityDetected, $"Record is activity detected {record}");

        long recordKnowledgeWatermark = record.KnowledgeWatermark;

        if (recordKnowledgeWatermark < this.knowledgeBase.Watermark)
        {
            foreach (var priorityGroup in this.knowledgeBase.PrioritizedDefinitions)
            {
                foreach (var definition in priorityGroup)
                {
                    if (recordKnowledgeWatermark >= definition.Id)
                    {
                        // skip the tested.
                        break;
                    }

                    var result = this.detectorsManager.GetDetector(definition.Id).Detect(record);

                    if (result?.ActivityInstance != null)
                    {
                        // Note: not update the watermark since a positive detection is not a full scan.
                        return result;
                    }
                }
            }
        }

        record.IsActivityDetected = false;
        record.KnowledgeWatermark = Math.Max(recordKnowledgeWatermark, this.knowledgeBase.Watermark);

        return new(record, null);
    }

    private IEnumerable<ActivityDetectionResult> Detect(IEnumerable<DataRecord> records) =>
        records.Select(r => this.Detect(r));
}
