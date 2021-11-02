namespace Encoo.ProcessMining.DB.Entities
{
    public record RuleData(
        MatchingOptions Matching,
        ExtractionOptions SubjectExtraction,
        ExtractionOptions ActorExtraction,
        ExtractionOptions TimeExtraction);
}
