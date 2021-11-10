using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Encoo.ProcessMining.Engine;

public class ProcessMiningEngine : EngineBase
{
    private readonly ILogger<ProcessMiningEngine> logger;
    private readonly IOptions<ProcessMiningEngineOptions> options;
    private readonly IProcessInstanceDetector processInstanceDetector;
    private readonly IProcessThumbprintIdentifier processClassifier;
    private readonly IProcessGroupDetector processGroupDetector;
    private readonly IProcessAnalyzer processAnalyzer;

    public ProcessMiningEngine(
        ILogger<ProcessMiningEngine> logger,
        IOptions<ProcessMiningEngineOptions> options,
        IServiceProvider serviceProvider,
        IProcessInstanceDetector processInstanceDetector,
        IProcessThumbprintIdentifier processClassifier,
        IProcessGroupDetector processGroupDetector,
        IProcessAnalyzer processAnalyzer)
        : base(logger, options, serviceProvider)
    {
        this.logger = logger;
        this.options = options;
        this.processInstanceDetector = processInstanceDetector;
        this.processClassifier = processClassifier;
        this.processGroupDetector = processGroupDetector;
        this.processAnalyzer = processAnalyzer;
    }

    protected override async Task<ExecuteUnitResult> ExecuteUnitAsync(IServiceProvider serviceProvider, CancellationToken token)
    {
        try
        {
            // step 1, detect process instance.
            var result = await this.processInstanceDetector.DetectAsync(token);

            if (result.Type != ExecuteUnitResultType.NoWorkToDo)
            {
                return result;
            }

            // step 2, Identify process thumbprint.
            result = await this.processClassifier.IdentifyAsync(token);

            if (result.Type != ExecuteUnitResultType.NoWorkToDo)
            {
                return result;
            }

            // step 3, Detect process group.
            result = await this.processGroupDetector.DetectAsync(token);

            if (result.Type != ExecuteUnitResultType.NoWorkToDo)
            {
                return result;
            }

            // step 3, BI analysis
            return await this.processAnalyzer.AnalyzeAsync(token);
        }
        catch (Exception ex)
        {
            return new ExecuteUnitResult(ExecuteUnitResultType.ExceptionHappened, ex);
        }
    }
}
