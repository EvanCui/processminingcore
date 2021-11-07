using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.DataContext;

public interface IKnowledgeBaseDataContext
{
    Task InitializeAsync(CancellationToken token);
    Task<KnowledgeBase> GetKnowledgeBaseAsync(bool forceReload, CancellationToken token);
    Task<long> GetKnowledgeWatermarkAsync(CancellationToken token);
    Task AddActivityDefinitionAsync(ActivityDefinition definition, CancellationToken token);
    Task RemoveActivityDefinitionAsync(long activityDefinitionId, CancellationToken token);
}
