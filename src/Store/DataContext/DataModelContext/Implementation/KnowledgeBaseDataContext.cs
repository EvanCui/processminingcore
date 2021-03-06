using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.DataContext.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Encoo.ProcessMining.DataContext;

public class KnowledgeBaseDataContext : IKnowledgeBaseDataContext
{
    private DateTimeOffset lastReloadTime = DateTimeOffset.MinValue;
    // TODO: config
    private readonly TimeSpan reloadInterval = TimeSpan.FromSeconds(30);
    private readonly IServiceProvider serviceProvider;
    private bool reloadNeeded = true;

    private KnowledgeBase knowledgeBase = null;

    public KnowledgeBaseDataContext(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<ActivityDefinition> AddActivityDefinitionAsync(ActivityDefinition definition, CancellationToken token)
    {
        using var scope = this.serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProcessMiningDatabaseContext>();

        context.ActivityDefinitions.Add(definition);
        await context.SaveChangesAsync(token);
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

            using var scope = this.serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProcessMiningDatabaseContext>();
            var definitions = await context.ActivityDefinitions.AsNoTracking().Include(d => d.ActivityDetectionRule).ToListAsync(token);

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
        using var scope = this.serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProcessMiningDatabaseContext>();
        context.ActivityDefinitions.Remove(new ActivityDefinition() { Id = activityDefinitionId });
        var deletedCount = await context.SaveChangesAsync(token);
        this.reloadNeeded = true;
        return deletedCount > 0;
    }

    public async Task<long> GetKnowledgeWatermarkAsync(CancellationToken token)
    {
        using var scope = this.serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProcessMiningDatabaseContext>();
        return await context.ActivityDefinitions.MaxAsync<ActivityDefinition, long?>(d => d.Id, token) ?? 0;
    }
}
