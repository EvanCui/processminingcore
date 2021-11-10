namespace Encoo.ProcessMining.DataContext;

public interface IProcessInstanceDataContext
{
    Task<int> SaveProcessThumbprintsAsync(IDictionary<long, string> thumbprints, CancellationToken cancellationToken);

    Task<int> AttachToProcessGroupsAsync(long batchSize, CancellationToken token);
}

