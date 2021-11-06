using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.Engine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Encoo.ProcessMining.Service.HostedService
{
    public class ActivityDetectionService : IHostedService
    {
        private readonly ILogger<ActivityDetectionService> logger;
        private readonly IServiceProvider serviceProvider;
        private ActivityDetectionEngine? engine;
        private IServiceScope? scope;

        public record DataItem(string FormattedText, string Template, int TemplateId, string[] Parameters, DateTimeOffset Time);

        public ActivityDetectionService(ILogger<ActivityDetectionService> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("service started");
            this.scope = this.serviceProvider.CreateScope();
            this.engine = this.scope.ServiceProvider.GetRequiredService<ActivityDetectionEngine>();


            if (false)
            {
#pragma warning disable CS0162 // Unreachable code detected
                var db = this.scope.ServiceProvider.GetRequiredService<ProcessMiningDatabaseContext>();
#pragma warning restore CS0162 // Unreachable code detected
                var json = File.ReadAllText("d:\\temp\\file1.json");
                //   JObject o = JObject.Parse(json);

                var dataItems = JsonConvert.DeserializeObject<List<DataItem>>(json);
                var records = dataItems!.Select(d => new DataContext.Model.DataRecord
                {
                    Template = d.Template,
                    ParametersArray = d.Parameters,
                    Content = d.FormattedText,
                    IsActivityDetected = false,
                    IsTemplateDetected = true,
                    IsDeleted = false,
                    Time = d.Time
                }).ToArray();

                int lastIndex = 0;
                int nextIndex = 0;
                while (nextIndex < records.Length)
                {
                    nextIndex = Math.Min(lastIndex + 10000, records.Length);
                    db.DataRecords.AddRange(records[lastIndex..nextIndex]);
                    await db.SaveChangesAsync(cancellationToken);
                    db.ChangeTracker.Clear();
                    lastIndex = nextIndex;
                    this.logger.LogInformation("Total {len}, inserted {lastIndex}", records.Length, lastIndex);
                }
            }

            await this.engine.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("service stopped");
            await this.engine!.StopAsync(cancellationToken);
            this.scope!.Dispose();
        }
    }
}
