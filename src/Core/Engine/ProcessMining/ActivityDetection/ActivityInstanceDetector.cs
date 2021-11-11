using Encoo.ProcessMining.DataContext;
using Encoo.ProcessMining.DataContext.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Encoo.ProcessMining.Engine;

public class ActivityInstanceDetector : IActivityInstanceDetector
{
    private readonly ILogger<ActivityInstanceDetector> logger;
    private readonly IKnowledgeBaseDataContext knowledgeBaseContext;
    private readonly IDataRecordDataContext dataRecordDataContext;
    private readonly IActivityInstanceDataContext activityInstanceDataContext;
    private readonly IActivityDetectorsManager detectorsManager = new ActivityDetectorsManager();
    private KnowledgeBase knowledgeBase;
    private bool forceReload = true;

    public ActivityInstanceDetector(
        ILogger<ActivityInstanceDetector> logger,
        IKnowledgeBaseDataContext knowledgeBaseContext,
        IDataRecordDataContext dataRecordDataContext,
        IActivityInstanceDataContext activityInstanceDataContext)
    {
        this.logger = logger;
        this.knowledgeBaseContext = knowledgeBaseContext;
        this.dataRecordDataContext = dataRecordDataContext;
        this.activityInstanceDataContext = activityInstanceDataContext;
    }

    public string Name => nameof(ActivityInstanceDetector);

    public async Task<RunResult> RunAsync(int batchSize, CancellationToken token)
    {
        try
        {
            this.knowledgeBase = await this.knowledgeBaseContext.GetKnowledgeBaseAsync(this.forceReload, token);
            this.forceReload = false;

            this.detectorsManager.Initialize(this.knowledgeBase.FlattenedDefinitions);

            var records = await this.dataRecordDataContext.LoadDataRecordToDetectAsync(
                this.knowledgeBase.Watermark,
                batchSize,
                token).ToListAsync(token);

            var results = records.AsParallel().Select(r => this.Detect(r));

            await this.activityInstanceDataContext.SaveActivityInstancesAsync(results.Where(r => r.ActivityInstance != null).Select(r => r.ActivityInstance), token);

            return records.Count == 0 ? RunResult.NoWorkToDo : RunResult.MoreWorkToDo;
        }
        catch (Exception ex)
        {
            this.forceReload = true;
            return RunResult.CreateErroredResult(ex);
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
}
