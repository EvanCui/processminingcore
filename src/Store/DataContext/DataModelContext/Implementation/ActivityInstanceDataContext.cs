using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.DataContext;

public class ActivityInstanceDataContext : IActivityInstanceDataContext
{
    private readonly ProcessMiningDatabaseContext databaseContext;

    public ActivityInstanceDataContext(ProcessMiningDatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public async Task SaveActivityInstancesAsync(IEnumerable<ActivityInstance> instances, CancellationToken cancellationToken)
    {
        await this.databaseContext.ActivityInstances.AddRangeAsync(instances, cancellationToken);
        await this.databaseContext.SaveChangesAsync(cancellationToken);
    }
}

