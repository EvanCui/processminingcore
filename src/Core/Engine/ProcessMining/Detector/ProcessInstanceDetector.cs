using Encoo.ProcessMining.DataContext;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Encoo.ProcessMining.Engine;

public class ProcessInstanceDetector : IProcessInstanceDetector
{
    private readonly ILogger<ProcessInstanceDetector> logger;
    private readonly IOptions<ProcessInstanceDetectorOptions> options;
    private readonly IActivityInstanceDataContext activtyInstanceDataContext;

    public ProcessInstanceDetector(ILogger<ProcessInstanceDetector> logger, IOptions<ProcessInstanceDetectorOptions> options, IActivityInstanceDataContext activtyInstanceDataContext)
    {
        this.logger = logger;
        this.options = options;
        this.activtyInstanceDataContext = activtyInstanceDataContext;
    }

    public async Task<ExecuteUnitResult> DetectAsync(CancellationToken token)
    {
        var attachedCount = await this.activtyInstanceDataContext.AttachToProcessInstancesAsync(this.options.Value.BatchSize, token);
        return new ExecuteUnitResult(attachedCount == 0 ? ExecuteUnitResultType.NoWorkToDo : ExecuteUnitResultType.MoreWorkToDo, null);
    }
}
