using Encoo.ProcessMining.Engine;

namespace Encoo.ProcessMining.Service.HostedService
{
    public class ActivityDetectionService : IHostedService
    {
        private readonly ILogger<ActivityDetectionService> logger;
        private readonly IServiceProvider serviceProvider;
        private ActivityDetectionEngine? engine;
        private IServiceScope? scope;

        public ActivityDetectionService(ILogger<ActivityDetectionService> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("service started");
            this.scope = this.serviceProvider.CreateScope();
            this.engine = this.scope.ServiceProvider.GetRequiredService<ActivityDetectionEngine>();
            return this.engine.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("service stopped");
            await this.engine!.StopAsync(cancellationToken);
            this.scope!.Dispose();
        }
    }
}
