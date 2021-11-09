using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Encoo.ProcessMining.Engine;

public class ProcessMiningEngine : EngineBase
{
    private readonly ILogger<ProcessMiningEngine> logger;
    private readonly IOptions<ProcessMiningEngineOptions> options;
    private readonly IProcessInstanceDetector processInstanceDetector;
    private readonly IProcessClassifier processClassifier;
    private readonly IProcessAnalyzer processAnalyzer;

    public ProcessMiningEngine(
        ILogger<ProcessMiningEngine> logger,
        IOptions<ProcessMiningEngineOptions> options,
        IServiceProvider serviceProvider,
        IProcessInstanceDetector processInstanceDetector,
        IProcessClassifier processClassifier,
        IProcessAnalyzer processAnalyzer)
        : base(logger, options, serviceProvider)
    {
        this.logger = logger;
        this.options = options;
        this.processInstanceDetector = processInstanceDetector;
        this.processClassifier = processClassifier;
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

            // step 2, classify process
            result = await this.processClassifier.ClassifyAsync(token);

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
