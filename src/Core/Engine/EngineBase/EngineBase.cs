using Encoo.ProcessMining.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Encoo.ProcessMining.Engine;

public abstract class EngineBase : IEngine
{
    private readonly ILogger<EngineBase> logger;
    private readonly IOptions<EngineOptions> options;
    private readonly IServiceProvider serviceProvider;
    private readonly CancellationTokenSource cancelExecute = new();
    private TaskCompletionSource trigger = new();

    public EngineBase(ILogger<EngineBase> logger, IOptions<EngineOptions> options, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.options = options;
        this.serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken token)
    {
        this.ExecuteAsync(token).FireAndForget();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken token)
    {
        this.cancelExecute.Cancel();
        return Task.CompletedTask;
    }

    public void TriggerExecute()
    {
        this.trigger.SetResult();
        this.trigger = new TaskCompletionSource();
    }

    protected abstract Task<ExecuteUnitResult> ExecuteUnitAsync(IServiceProvider serviceProvider, CancellationToken token);

    private async Task ExecuteAsync(CancellationToken token)
    {
        token = CancellationTokenSource.CreateLinkedTokenSource(token, this.cancelExecute.Token).Token;
        while (!token.IsCancellationRequested)
        {
            var triggerExecute = this.trigger;
            var scope = this.serviceProvider.CreateScope();

            ExecuteUnitResult result;

            try
            {
                result = await this.ExecuteUnitAsync(scope.ServiceProvider, token);
            }
            finally
            {
                scope.Dispose();
            }

            int waitSeconds = result.Type switch
            {
                ExecuteUnitResultType.NoWorkToDo => this.options.Value.IdleWaitSeconds,
                ExecuteUnitResultType.ExceptionHappened => this.options.Value.ErrorWaitSeconds,
                _ => 0,
            };

            if (result.Type == ExecuteUnitResultType.ExceptionHappened)
            {
                this.logger.LogError("Exception captured {0}", result.Exception);
            }

            await Task.WhenAny(Task.Delay(TimeSpan.FromSeconds(waitSeconds), token), triggerExecute.Task);
        }
    }
}
