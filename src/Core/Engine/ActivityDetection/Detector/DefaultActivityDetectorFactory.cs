using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    class DefaultActivityDetectorFactory : IActivityDetectorFactory
    {
        private readonly IMatcherFactory matcherFactory = new DefaultMatcherFactory();
        private readonly IExtractorFactory extractorFactory = new DefaultExtractorFactory();

        public IActivityDetector CreateActivityDetector(ActivityDefinition definition)
        {
            var ruleData = definition.Rule.Data;

            return new DefaultActivityDetector(
                definition.Rule.Id.Value,
                definition.Id.Value,
                this.matcherFactory.CreateMatcher(ruleData.Matching),
                this.extractorFactory.CreateSubjectExtractor(ruleData.SubjectExtraction),
                this.extractorFactory.CreateActorExtractor(ruleData.ActorExtraction),
                this.extractorFactory.CreateTimeExtractor(ruleData.TimeExtraction));
        }
    }
}
