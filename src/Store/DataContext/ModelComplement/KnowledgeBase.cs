namespace Encoo.ProcessMining.DataContext.Model;

public record KnowledgeBase(
    long Watermark,
    List<IGrouping<int, ActivityDefinition>> PrioritizedDefinitions)
{
    public IEnumerable<ActivityDefinition> FlattenedDefinitions => this.PrioritizedDefinitions.SelectMany(g => g);
}
