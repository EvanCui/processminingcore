using Encoo.ProcessMining.DataContext.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Encoo.ProcessMining.DataContext;

public class ProcessInstanceDataContext : IProcessInstanceDataContext
{
    private readonly ProcessMiningDatabaseContext databaseContext;

    public ProcessInstanceDataContext(ProcessMiningDatabaseContext processMiningDatabaseContext)
    {
        this.databaseContext = processMiningDatabaseContext;
    }

    public Task<int> AttachToProcessGroupsAsync(long batchSize, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task<int> SaveProcessThumbprintsAsync(IDictionary<long, string> thumbprints, CancellationToken cancellationToken)
    {
        var processInstances = await this.databaseContext.ProcessInstances.AsQueryable()
            .Where(pi => pi.IsGrouped == false)
            .ToListAsync(cancellationToken);

        processInstances.ForEach(p =>
        {
            if (thumbprints.TryGetValue(p.Id, out var thumprint))
            {
                p.Thumbprint = thumprint;
            }
        });

        return await this.databaseContext.SaveChangesAsync(cancellationToken);
    }
}
