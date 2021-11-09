using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.DataContext;

public interface IActivityInstanceDataContext
{
    Task SaveActivityInstancesAsync(IEnumerable<ActivityInstance> instances, CancellationToken cancellationToken);

    Task<int> AttachToProcessInstancesAsync(long limitCount, CancellationToken token);
}

