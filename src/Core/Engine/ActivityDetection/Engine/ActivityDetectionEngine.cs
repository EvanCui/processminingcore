using Encoo.ProcessMining.DB.Core;
using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    class ActivityDetectionEngine
    {
        private readonly IDataContext dataContext;
        private readonly ActivityDetectionEngineOptions options;
        private readonly IActivityDetectorsManager detectorsManager = new ActivityDetectorsManager();
        private ActivityDefinitionKnowledgeBase knowledgeBase;

        public ActivityDetectionEngine(ActivityDetectionEngineOptions options, IDataContext dataContext)
        {
            this.options = options;
            this.dataContext = dataContext;
        }

        private ActivityDetectionResult Detect(DataRecord record)
        {
            long recordKnowledgeWatermark = 0;

            if (record.Status == DataRecordStatus.Unmatched)
            {
                recordKnowledgeWatermark = record.KnowledgeWatermark;
            }
            else
            {
                Debug.Assert(record.Status == DataRecordStatus.TemplateDetected,
                    $"Unexpected record status in {nameof(ActivityDetectionEngine.Detect)} for {record}");
            }

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

                        var result = this.detectorsManager.GetDetector(definition.Id.Value).Detect(record);

                        if (result?.ActivityInstance != null)
                        {
                            // Note: not update the watermark since a positive detection is not a full scan.
                            return result;
                        }
                    }
                }
            }

            return new(
                record with
                {
                    Status = DataRecordStatus.Unmatched,
                    KnowledgeWatermark = Math.Max(this.knowledgeBase.Watermark, recordKnowledgeWatermark),
                },
                null);
        }

        private IEnumerable<ActivityDetectionResult> Detect(IEnumerable<DataRecord> records) =>
            records.Select(r => this.Detect(r));

        public async Task ExecuteAsync(CancellationToken token)
        {
            bool forceReload = true;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    this.knowledgeBase = await this.dataContext.GetKnowledgeBaseAsync(forceReload, token);
                    this.detectorsManager.Initialize(this.knowledgeBase.FlattenedDefinitions);
                    forceReload = false;

                    var records = this.dataContext.LoadDataRecordAsync(
                        this.knowledgeBase.Watermark,
                        this.options.BatchLoadingSize,
                        token);

                    var recordsList = new List<DataRecord>();

                    await foreach (var record in records)
                    {
                        recordsList.Add(record);
                    }

                    var workItemSize = recordsList.Count / Environment.ProcessorCount;
                    workItemSize = Math.Max(1, workItemSize);
                    workItemSize = Math.Min(this.options.MaxWorkItemSize, workItemSize);

                    var recordGroups = Enumerable.Range(0, recordsList.Count).GroupBy(i => i / workItemSize, i => recordsList[i]);

                    var results = await Task.WhenAll(recordGroups.Select(recordGroup => Task.Run(() => this.Detect(recordGroup))));

                    await this.dataContext.SaveDetectionResultAsync(results.SelectMany(r => r), token);
                }
                catch (Exception)
                {
                    // TODO: log error
                    forceReload = true;
                }
            }
        }
    }
}
