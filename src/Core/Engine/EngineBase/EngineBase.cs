using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Encoo.ProcessMining.Engine;

public abstract class EngineBase : IEngine
{
    private readonly ILogger<EngineBase> logger;
    private readonly IOptions<EngineBaseOptions> options;

    public EngineBase(ILogger<EngineBase> logger, IOptions<EngineBaseOptions> options)
    {
        this.logger = logger;
        this.options = options;
    }

    public abstract IList<IEngineComponent> EngineComponents { get; init; }

    public async Task<RunResult> RunAsync(CancellationToken cancellationToken)
    {
        var completedParts = 0;

        try
        {
            foreach (var engineComponent in EngineComponents)
            {
                var result = await engineComponent.RunAsync(this.options.Value.BatchSize, cancellationToken);
                this.logger.LogInformation("Component {name} executed with result {result}", engineComponent.Name, result);

                if (result.Type != RunResultType.NoWorkToDo)
                {
                    completedParts++;
                    return result with { Progress = this.GetProgress(completedParts) };
                }

                completedParts += 2;
            }

            return RunResult.Completed;
        }
        catch (Exception ex)
        {
            return new RunResult(RunResultType.ExceptionHappened,  this.GetProgress(completedParts), ex);
        }
    }

    private int GetProgress(int completedParts) => 50 * completedParts / this.EngineComponents.Count;
}
