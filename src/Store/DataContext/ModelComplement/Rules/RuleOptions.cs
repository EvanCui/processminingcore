namespace Encoo.ProcessMining.DataContext.Model;

public record RuleOptions(
    MatchingOptions Matching,
    ExtractionOptions SubjectExtraction,
    ExtractionOptions ActorExtraction,
    ExtractionOptions TimeExtraction);
