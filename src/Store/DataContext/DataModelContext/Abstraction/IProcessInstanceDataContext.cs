namespace Encoo.ProcessMining.DataContext;

public interface IProcessInstanceDataContext
{
    Task<int> SaveProcessThumbprintsAsync(IDictionary<long, string> thumbprints, CancellationToken cancellationToken);

    Task<int> AttachToProcessClustersAsync(long batchSize, CancellationToken token);
}

