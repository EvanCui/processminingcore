using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.DataContext.Model;
using Microsoft.EntityFrameworkCore;

namespace Encoo.ProcessMining.DataContext;

public class ActivityInstanceDataContext : IActivityInstanceDataContext
{
    private readonly ProcessMiningDatabaseContext databaseContext;

    public ActivityInstanceDataContext(ProcessMiningDatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public Task<int> AttachToProcessInstancesAsync(long limitCount, CancellationToken token) =>
        this.databaseContext.Database.ExecuteSqlRawAsync("AttachActivityInstanceToProcessInstance {0}", limitCount);

    public async Task SaveActivityInstancesAsync(IEnumerable<ActivityInstance> instances, CancellationToken cancellationToken)
    {
        await this.databaseContext.ActivityInstances.AddRangeAsync(instances, cancellationToken);
        await this.databaseContext.SaveChangesAsync(cancellationToken);
    }
}

