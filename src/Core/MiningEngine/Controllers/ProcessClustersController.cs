using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.DataContext.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Encoo.ProcessMining.Service.Controllers
{
    [Route("v1/ProcessDefinitions/{definitionId}/[controller]")]
    [ApiController]
    public class ProcessClustersController : ControllerBase
    {
        private readonly ProcessMiningDatabaseContext processMiningDatabase;

        public ProcessClustersController(ProcessMiningDatabaseContext processMiningDatabase)
        {
            this.processMiningDatabase = processMiningDatabase;
        }

        [HttpGet(Name = "GetProcessClusters")]
        public IAsyncEnumerable<ProcessCluster> GetProcessClustersAsync(long definitionId, CancellationToken token)
        {
            return this.processMiningDatabase.ActivityDefinitions
                .AsQueryable()
                .Where(d => d.ProcessDefinitionId == definitionId)
                .Join(this.processMiningDatabase.ProcessClusterActivityDefinitionRelationships, d => d.Id, r => r.ActivityDefinitionId, (d, r) => r)
                .Join(this.processMiningDatabase.ProcessClusters, r => r.ProcessClusterId, c => c.Id, (r, c) => c)
                .ToAsyncEnumerable();
        }
    }
}
