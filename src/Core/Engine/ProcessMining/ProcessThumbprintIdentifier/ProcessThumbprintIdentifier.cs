using Encoo.ProcessMining.DataContext.DatabaseContext;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using Encoo.ProcessMining.DataContext;

namespace Encoo.ProcessMining.Engine.ProcessMining.Classifier;

internal class ProcessThumbprintIdentifier : IProcessThumbprintIdentifier
{
    private readonly ILogger<ProcessThumbprintIdentifier> logger;
    private readonly IOptions<ProcessThumbprintIdentifierOptions> options;
    private readonly ProcessMiningDatabaseContext databaseContext;
    private readonly IProcessInstanceDataContext processInstanceDataContext;

    public ProcessThumbprintIdentifier(
        ILogger<ProcessThumbprintIdentifier> logger,
        IOptions<ProcessThumbprintIdentifierOptions> options,
        ProcessMiningDatabaseContext databaseContext,
        IProcessInstanceDataContext processInstanceDataContext)
    {
        this.logger = logger;
        this.options = options;
        this.databaseContext = databaseContext;
        this.processInstanceDataContext = processInstanceDataContext;
    }

    public async Task<ExecuteUnitResult> IdentifyAsync(CancellationToken token)
    {
        // Step 1, calculate the thumbprints for processes
        var processActivityDefinitionIds = await this.databaseContext.ProcessInstances.AsQueryable()
            .Where(pi => pi.IsGrouped == false)
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
        return new ExecuteUnitResult(savedCount == 0 ? ExecuteUnitResultType.NoWorkToDo : ExecuteUnitResultType.MoreWorkToDo, null);
    }
}
