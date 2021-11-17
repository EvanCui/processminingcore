using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.DataContext.Model;
using Microsoft.EntityFrameworkCore;

namespace Encoo.ProcessMining.DataContext;

public class KnowledgeBaseDataContext : IKnowledgeBaseDataContext
{
    private readonly ProcessMiningDatabaseContext context;

    private DateTimeOffset lastReloadTime = DateTimeOffset.MinValue;
    // TODO: config
    private readonly TimeSpan reloadInterval = TimeSpan.FromSeconds(30);
    private bool reloadNeeded = true;

    private KnowledgeBase knowledgeBase = null;

    public KnowledgeBaseDataContext(ProcessMiningDatabaseContext context)
    {
        this.context = context;
    }

    public async Task<ActivityDefinition> AddActivityDefinitionAsync(ActivityDefinition definition, CancellationToken token)
    {
        this.context.ActivityDefinitions.Add(definition);
        await this.context.SaveChangesAsync(token);
        this.reloadNeeded = true;
        return definition;
    }

    public async Task<KnowledgeBase> GetKnowledgeBaseAsync(bool forceReload, CancellationToken token)
    {
        // TODO: single load
        if (forceReload || this.reloadNeeded || this.knowledgeBase == null || DateTimeOffset.UtcNow - this.lastReloadTime > reloadInterval)
        {
            this.lastReloadTime = DateTimeOffset.UtcNow;
            this.reloadNeeded = false;

            var definitions = await this.context.ActivityDefinitions.AsNoTracking().Include(d => d.ActivityDetectionRule).ToListAsync(token);

            var watermark = definitions.Max(d => d.Id);
            this.knowledgeBase = new KnowledgeBase(
                watermark,
                definitions
                    .OrderByDescending(d => d.Id)
                    .GroupBy(d => d.ActivityDetectionRule.Priority)
                    .ToList());
        }

        return this.knowledgeBase;
    }

    public Task InitializeAsync(CancellationToken token) =>
        this.GetKnowledgeBaseAsync(true, token);

    public async Task<bool> RemoveActivityDefinitionAsync(long activityDefinitionId, CancellationToken token)
    {
        this.context.ActivityDefinitions.Remove(new ActivityDefinition() { Id = activityDefinitionId });
        var deletedCount = await this.context.SaveChangesAsync(token);
        this.reloadNeeded = true;
        return deletedCount > 0;
    }

    public Task<long> GetKnowledgeWatermarkAsync(CancellationToken token)
    {
        return this.context.ActivityDefinitions.MaxAsync(d => d.Id, token);
    }
}
