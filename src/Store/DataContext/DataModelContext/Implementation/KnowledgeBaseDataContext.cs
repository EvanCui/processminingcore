using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.DataContext.Model;
using Microsoft.EntityFrameworkCore;

namespace Encoo.ProcessMining.DataContext;

public class KnowledgeBaseDataContext : IKnowledgeBaseDataContext
{
    private readonly ProcessMiningDatabaseContext context;

    private DateTimeOffset lastReloadTime = DateTimeOffset.MinValue;
    // TODO: config
    private TimeSpan reloadInterval = TimeSpan.FromSeconds(30);
    private bool reloadNeeded = true;

    private KnowledgeBase knowledgeBase = null;

    public KnowledgeBaseDataContext(ProcessMiningDatabaseContext context)
    {
        this.context = context;
    }

    public async Task AddActivityDefinitionAsync(ActivityDefinition definition, CancellationToken token)
    {
        this.context.ActivityDefinitions.Add(definition);
        await this.context.SaveChangesAsync(token);
        this.reloadNeeded = true;
    }

    public async Task<KnowledgeBase> GetKnowledgeBaseAsync(bool forceReload, CancellationToken token)
    {
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

    public async Task RemoveActivityDefinitionAsync(long activityDefinitionId, CancellationToken token)
    {
        this.context.ActivityDefinitions.Remove(new ActivityDefinition() { Id = activityDefinitionId });
        await this.context.SaveChangesAsync(token);
        this.reloadNeeded = true;
    }
}
