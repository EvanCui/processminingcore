using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.Engine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Encoo.ProcessMining.Service.HostedService
{
    public class ProcessMiningService : IHostedService
    {
        private readonly ILogger<ProcessMiningService> logger;
        private readonly IEngineController engineController;

        public record DataItem(string FormattedText, string Template, int TemplateId, string[] Parameters, DateTimeOffset Time);

        public ProcessMiningService(ILogger<ProcessMiningService> logger, IEngineController engineController)
        {
            this.logger = logger;
            this.engineController = engineController;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("service started");
            await this.engineController.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("service stopped");
            await this.engineController.StopAsync(cancellationToken);
        }
    }
}
