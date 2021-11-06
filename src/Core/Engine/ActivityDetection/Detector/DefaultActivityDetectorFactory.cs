using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

class DefaultActivityDetectorFactory : IActivityDetectorFactory
{
    private readonly IMatcherFactory matcherFactory = new DefaultMatcherFactory();
    private readonly IExtractorFactory extractorFactory = new DefaultExtractorFactory();

    public IActivityDetector CreateActivityDetector(ActivityDefinition definition)
    {
        var ruleOptions = definition.ActivityDetectionRule.RuleOptions;

        return new DefaultActivityDetector(
            definition.ActivityDetectionRule.Id,
            definition.Id,
            this.matcherFactory.CreateMatcher(ruleOptions.Matching),
            this.extractorFactory.CreateSubjectExtractor(ruleOptions.SubjectExtraction),
            this.extractorFactory.CreateActorExtractor(ruleOptions.ActorExtraction),
            this.extractorFactory.CreateTimeExtractor(ruleOptions.TimeExtraction));
    }
}
