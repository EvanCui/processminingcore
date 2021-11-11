using Encoo.ProcessMining.DataContext;
using Microsoft.Extensions.Logging;

namespace Encoo.ProcessMining.Engine;

public class ProcessInstanceDetector : IProcessInstanceDetector
{
    private readonly ILogger<ProcessInstanceDetector> logger;
    private readonly IActivityInstanceDataContext activtyInstanceDataContext;

    public ProcessInstanceDetector(ILogger<ProcessInstanceDetector> logger, IActivityInstanceDataContext activtyInstanceDataContext)
    {
        this.logger = logger;
        this.activtyInstanceDataContext = activtyInstanceDataContext;
    }

    public string Name => nameof(ProcessInstanceDetector);

    public async Task<RunResult> RunAsync(int batchSize, CancellationToken token)
    {
        var attachedCount = await this.activtyInstanceDataContext.AttachToProcessInstancesAsync(batchSize, token);
        return attachedCount == 0 ? RunResult.NoWorkToDo : RunResult.MoreWorkToDo;
    }
}
