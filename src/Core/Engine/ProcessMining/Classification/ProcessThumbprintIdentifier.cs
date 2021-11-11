using Encoo.ProcessMining.DataContext.DatabaseContext;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using Encoo.ProcessMining.DataContext;

namespace Encoo.ProcessMining.Engine;

internal class ProcessThumbprintIdentifier : IProcessThumbprintIdentifier
{
    private readonly ILogger<ProcessThumbprintIdentifier> logger;
    private readonly ProcessMiningDatabaseContext databaseContext;
    private readonly IProcessInstanceDataContext processInstanceDataContext;

    public ProcessThumbprintIdentifier(
        ILogger<ProcessThumbprintIdentifier> logger,
        ProcessMiningDatabaseContext databaseContext,
        IProcessInstanceDataContext processInstanceDataContext)
    {
        this.logger = logger;
        this.databaseContext = databaseContext;
        this.processInstanceDataContext = processInstanceDataContext;
    }

    public string Name => nameof(ProcessThumbprintIdentifier);

    public async Task<RunResult> RunAsync(int batchSize, CancellationToken token)
    {
        var processActivityDefinitionIds = await this.databaseContext.ProcessInstances.AsQueryable()
            .Where(pi => pi.IsClustered == false)
            .Join(this.databaseContext.ActivityInstances, pi => pi.Id, ai => ai.ProcessInstanceId, (pi, ai) => new { ProcessInstanceId = pi.Id, ai.ActivityDefinitionId, ai.Time })
            .ToListAsync(token);

        var thumbprints = processActivityDefinitionIds
            .OrderBy(p => p.Time)
            .GroupBy(p => p.ProcessInstanceId)
            .AsParallel()
            .Select(g => new
            {
                ProcessInstanceId = g.Key,
                Thumbprint = JsonConvert.SerializeObject(g.Select(p => p.ActivityDefinitionId).ToArray()),
            })
            .Select(g => new
            {
                g.ProcessInstanceId,
                Thumbprint = BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(g.Thumbprint))),
            })
            .ToDictionary(p => p.ProcessInstanceId, p => p.Thumbprint);

        var savedCount = await this.processInstanceDataContext.SaveProcessThumbprintsAsync(thumbprints, token);
        return savedCount == 0 ? RunResult.NoWorkToDo : RunResult.MoreWorkToDo;
    }
}
