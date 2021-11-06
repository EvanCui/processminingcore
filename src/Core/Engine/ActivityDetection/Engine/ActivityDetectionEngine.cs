using Encoo.ProcessMining.DataContext;
using Encoo.ProcessMining.DataContext.Model;
using Encoo.ProcessMining.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace Encoo.ProcessMining.Engine;

public class ActivityDetectionEngine
{
    private readonly ILogger<ActivityDetectionEngine> logger;
    private readonly ActivityDetectionEngineOptions options;
    private readonly IKnowledgeBaseDataContext knowledgeBaseContext;
    private readonly IDataRecordDataContext dataRecordDataContext;
    private readonly IActivityInstanceDataContext activityInstanceDataContext;
    private readonly IActivityDetectorsManager detectorsManager = new ActivityDetectorsManager();
    private KnowledgeBase knowledgeBase;
    private TaskCompletionSource triggerExecute = new();
    private readonly CancellationTokenSource cancelExecute = new();

    public ActivityDetectionEngine(
        ILogger<ActivityDetectionEngine> logger,
        IOptions<ActivityDetectionEngineOptions> options,
        IKnowledgeBaseDataContext knowledgeBaseContext,
        IDataRecordDataContext dataRecordDataContext,
        IActivityInstanceDataContext activityInstanceDataContext)
    {
        this.logger = logger;
        this.options = options.Value;
        this.knowledgeBaseContext = knowledgeBaseContext;
        this.dataRecordDataContext = dataRecordDataContext;
        this.activityInstanceDataContext = activityInstanceDataContext;
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

    public Task StopAsync(CancellationToken token)
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

            try
            {
                this.knowledgeBase = await this.knowledgeBaseContext.GetKnowledgeBaseAsync(forceReload, token);
                forceReload = false;

                this.detectorsManager.Initialize(this.knowledgeBase.FlattenedDefinitions);

                var records = await this.dataRecordDataContext.LoadDataRecordToDetectAsync(
                    this.knowledgeBase.Watermark,
                    this.options.BatchLoadingSize,
                    token).ToListAsync(token);

                var workItemSize = records.Count / Environment.ProcessorCount;
                workItemSize = Math.Max(1, workItemSize);
                workItemSize = Math.Min(this.options.MaxWorkItemSize, workItemSize);

                var recordGroups = Enumerable.Range(0, records.Count).GroupBy(i => i / workItemSize, i => records[i]);

                var results = await Task.WhenAll(recordGroups.Select(recordGroup => Task.Run(() => this.Detect(recordGroup))));

                await this.SaveDetectionResultsAsync(results.SelectMany(r => r), token);

                await Task.WhenAny(Task.Delay(TimeSpan.FromSeconds(this.options.DetectionIntervalSeconds), token), trigger.Task);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception happened {ex}", ex);
                forceReload = true;
                await Task.WhenAny(Task.Delay(TimeSpan.FromSeconds(this.options.ErrorRetryIntervalSeconds), token), trigger.Task);
            }
        }
    }

    private async Task SaveDetectionResultsAsync(IEnumerable<ActivityDetectionResult> results, CancellationToken token)
    {
        await this.activityInstanceDataContext.SaveActivityInstancesAsync(results.Select(r => r.ActivityInstance), token);
    }

    private ActivityDetectionResult Detect(DataRecord record)
    {
        Debug.Assert(!record.IsDeleted, $"Record is deleted {record}");
        Debug.Assert(record.IsTemplateDetected, $"Record is not template detected {record}");
        Debug.Assert(!record.IsActivityDetected, $"Record is activity detected {record}");

        long recordKnowledgeWatermark = record.KnowledgeWatermark ?? 0;

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
