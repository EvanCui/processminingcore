using Encoo.ProcessMining.DataContext;
using Microsoft.Extensions.Logging;

namespace Encoo.ProcessMining.Engine;

public class ProcessClusterDetector : IProcessClusterDetector
{
    private readonly ILogger<ProcessClusterDetector> logger;
    private readonly IProcessInstanceDataContext processInstanceDataContext;

    public ProcessClusterDetector(ILogger<ProcessClusterDetector> logger, IProcessInstanceDataContext processInstanceDataContext)
    {
        this.logger = logger;
        this.processInstanceDataContext = processInstanceDataContext;
    }

    public string Name => nameof(ProcessClusterDetector);

    public async Task<RunResult> RunAsync(int batchSize, CancellationToken token)
    {
        var attachedCount = await this.processInstanceDataContext.AttachToProcessClustersAsync(batchSize, token);
        return attachedCount == 0 ? RunResult.NoWorkToDo : RunResult.MoreWorkToDo;
    }
}
