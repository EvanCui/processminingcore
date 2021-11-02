namespace Encoo.ProcessMining.DB.Entities
{
    public record ActivityDetectionRule(
        long? Id,
        string Name,
        int Priority,
        RuleData Data) : IntIdRecord(Id)
    {
        public long ActivityDefinitionId { get; set; }
    }
}
