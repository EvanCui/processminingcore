using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    class DefaultActivityDetector : IActivityDetector
    {
        private readonly long ruleId;
        private readonly long activityDefinitionId;
        private readonly IMatcher matcher;
        private readonly IExtractor<string> subjectExtractor;
        private readonly IExtractor<string> actorExtractor;
        private readonly IExtractor<DateTimeOffset> timeExtractor;

        public DefaultActivityDetector(
            long ruleId,
            long activityDefinitionId,
            IMatcher matcher,
            IExtractor<string> subjectExtractor,
            IExtractor<string> actorExtractor,
            IExtractor<DateTimeOffset> timeExtractor)
        {
            this.ruleId = ruleId;
            this.activityDefinitionId = activityDefinitionId;
            this.matcher = matcher;
            this.subjectExtractor = subjectExtractor;
            this.actorExtractor = actorExtractor;
            this.timeExtractor = timeExtractor;
        }

        public ActivityDetectionResult Detect(DataRecord dataRecord)
        {
            var contentData = dataRecord.Data;
            (var success, var tokens) = this.matcher.Match(contentData);
            if (!success) return null;

            return new(dataRecord with { Status = DataRecordStatus.ActivityDetected },
                new ActivityInstance(
                    null,
                    this.actorExtractor.Extract(contentData, tokens),
                    this.timeExtractor.Extract(contentData, tokens),
                    this.subjectExtractor.Extract(contentData, tokens),
                    this.ruleId,
                    this.activityDefinitionId,
                    dataRecord.Id.Value,
                    null));
        }
    }
}
