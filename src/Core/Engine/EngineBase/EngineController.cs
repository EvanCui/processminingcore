using Encoo.ProcessMining.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Encoo.ProcessMining.Engine;

public class EngineController<T> : IEngineController where T : IEngineBuilder
{
    private readonly ILogger<EngineController<T>> logger;
    private readonly IOptions<EngineControllerOptions> options;
    private readonly IServiceProvider serviceProvider;
    private readonly CancellationTokenSource cancelExecute = new();
    private TaskCompletionSource trigger = new();

    public IEngineBuilder EngineBuilder { get; private set; }

    public EngineController(ILogger<EngineController<T>> logger, IOptions<EngineControllerOptions> options, T engineBuilder, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.options = options;
        this.EngineBuilder = engineBuilder;
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

    private async Task ExecuteAsync(CancellationToken token)
    {
        token = CancellationTokenSource.CreateLinkedTokenSource(token, this.cancelExecute.Token).Token;
        while (!token.IsCancellationRequested)
        {
            var triggerExecute = this.trigger;
            var scope = this.serviceProvider.CreateScope();

            RunResult result;

            try
            {
                result = await this.EngineBuilder.Build(scope.ServiceProvider).RunAsync(token);
            }
            finally
            {
                scope.Dispose();
            }

            int waitSeconds = result.Type switch
            {
                RunResultType.NoWorkToDo => this.options.Value.IdleWaitSeconds,
                RunResultType.ExceptionHappened => this.options.Value.ErrorWaitSeconds,
                _ => 0,
            };

            this.logger.LogError("Run result {result}", result);
            if (result.Type == RunResultType.ExceptionHappened)
            {
                this.logger.LogError("Exception captured {exception}", result.Exception);
            }

            await Task.WhenAny(Task.Delay(TimeSpan.FromSeconds(waitSeconds), token), triggerExecute.Task);
        }
    }
}
