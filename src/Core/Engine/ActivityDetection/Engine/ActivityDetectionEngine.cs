using Encoo.ProcessMining.DataContext;
using Encoo.ProcessMining.DataContext.Model;
using Encoo.ProcessMining.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Encoo.ProcessMining.Engine;

public class ActivityDetectionEngine : EngineBase
{
    private readonly ILogger<ActivityDetectionEngine> logger;
    private readonly ActivityDetectionEngineOptions options;
    private readonly IKnowledgeBaseDataContext knowledgeBaseContext;
    private readonly IActivityDetectorsManager detectorsManager = new ActivityDetectorsManager();
    private KnowledgeBase knowledgeBase;
    private bool forceReload = true;

    public ActivityDetectionEngine(
        ILogger<ActivityDetectionEngine> logger,
        IOptions<ActivityDetectionEngineOptions> options,
        IServiceProvider serviceProvider,
        IKnowledgeBaseDataContext knowledgeBaseContext)
        : base(logger, options, serviceProvider)
    {
        this.logger = logger;
        this.options = options.Value;
        this.knowledgeBaseContext = knowledgeBaseContext;
    }

    protected override async Task<ExecuteUnitResult> ExecuteUnitAsync(IServiceProvider serviceProvider, CancellationToken token)
    {
        try
        {
            this.knowledgeBase = await this.knowledgeBaseContext.GetKnowledgeBaseAsync(this.forceReload, token);
            this.forceReload = false;

            this.detectorsManager.Initialize(this.knowledgeBase.FlattenedDefinitions);

            var dataRecordDataContext = serviceProvider.GetRequiredService<IDataRecordDataContext>();

            var records = await dataRecordDataContext.LoadDataRecordToDetectAsync(
                this.knowledgeBase.Watermark,
                this.options.BatchLoadingSize,
                token).ToListAsync(token);

            var workItemSize = records.Count / Environment.ProcessorCount;
            workItemSize = Math.Max(1, workItemSize);
            workItemSize = Math.Min(this.options.MaxWorkItemSize, workItemSize);

            var recordGroups = Enumerable.Range(0, records.Count).GroupBy(i => i / workItemSize, i => records[i]);

            var results = await Task.WhenAll(recordGroups.Select(recordGroup => Task.Run(() => this.Detect(recordGroup))));

            var activityInstanceDataContext = serviceProvider.GetRequiredService<IActivityInstanceDataContext>();
            await activityInstanceDataContext.SaveActivityInstancesAsync(results.SelectMany(r => r).Where(r => r.ActivityInstance != null).Select(r => r.ActivityInstance), token);

            return new ExecuteUnitResult(records.Count == 0 ? ExecuteUnitResultType.NoWorkToDo : ExecuteUnitResultType.MoreWorkToDo, null);
        }
        catch (Exception ex)
        {
            this.forceReload = true;
            return new ExecuteUnitResult(ExecuteUnitResultType.ExceptionHappened, ex);
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
