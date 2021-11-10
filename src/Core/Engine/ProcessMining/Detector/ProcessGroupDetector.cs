using Encoo.ProcessMining.DataContext;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Encoo.ProcessMining.Engine;

public class ProcessGroupDetector : IProcessGroupDetector
{
    private readonly ILogger<ProcessGroupDetector> logger;
    private readonly IOptions<ProcessGroupDetectorOptions> options;
    private readonly IProcessInstanceDataContext processInstanceDataContext;

    public ProcessGroupDetector(ILogger<ProcessGroupDetector> logger, IOptions<ProcessGroupDetectorOptions> options, IProcessInstanceDataContext processInstanceDataContext)
    {
        this.logger = logger;
        this.options = options;
        this.processInstanceDataContext = processInstanceDataContext;
    }

    public async Task<ExecuteUnitResult> DetectAsync(CancellationToken token)
    {
        var attachedCount = await this.processInstanceDataContext.AttachToProcessGroupsAsync(this.options.Value.BatchSize, token);
        return new ExecuteUnitResult(attachedCount == 0 ? ExecuteUnitResultType.NoWorkToDo : ExecuteUnitResultType.MoreWorkToDo, null);
    }
}
