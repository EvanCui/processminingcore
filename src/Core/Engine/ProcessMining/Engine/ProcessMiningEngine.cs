using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Encoo.ProcessMining.Engine;

public class ProcessMiningEngine : EngineBase
{
    public ProcessMiningEngine(
        ILogger<ProcessMiningEngine> logger,
        IOptions<ProcessMiningEngineOptions> options,
        IActivityInstanceDetector activityInstanceDetector,
        IProcessInstanceDetector processInstanceDetector,
        IProcessThumbprintIdentifier processClassifier,
        IProcessClusterDetector processClusterDetector,
        IProcessAnalyzer processAnalyzer)
        : base(logger, options)
    {
        this.EngineComponents = new List<IEngineComponent>()
        {
            // step 1, detect activity instance.
            activityInstanceDetector,
            // step 2, detect process instance.
            processInstanceDetector,
            // step 3, identify process thumbprint.
            processClassifier,
            // step 4, detect process cluster.
            processClusterDetector,
            // step 5, analysis.
            processAnalyzer,
        };
    }

    public override IList<IEngineComponent> EngineComponents { get; init; }
}
