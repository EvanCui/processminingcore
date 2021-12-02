using Encoo.ProcessMining.DataContext;
using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.DataContext.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Encoo.ProcessMining.Service.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ProcessDefinitionsController : ControllerBase
    {
        private readonly ProcessMiningDatabaseContext processMiningDatabase;

        public ProcessDefinitionsController(ProcessMiningDatabaseContext processMiningDatabase)
        {
            this.processMiningDatabase = processMiningDatabase;
        }

        // GET: ProcessDefinitionsController
        [HttpGet(Name = "GetProcessDefinitions")]
        public IEnumerable<ProcessDefinition> GetProcessDefinitionsAsync(CancellationToken token)
        {
            return this.processMiningDatabase.ProcessDefinitions;
        }

        [HttpPost(Name = "AddProcessDefinitions")]
        public async Task<ProcessDefinition[]> AddProcessDefinitionsAsync([FromBody] ProcessDefinition[] definitions, CancellationToken token)
        {
            await this.processMiningDatabase.ProcessDefinitions.AddRangeAsync(definitions, token);
            await this.processMiningDatabase.SaveChangesAsync(token);
            return definitions;
        }

        [HttpDelete(Name = "RemoveProcessDefinitions")]
        public async Task<int> RemoveProcessDefinitionsAsync([FromBody] long[] processDefinitionIds, CancellationToken token)
        {
            this.processMiningDatabase.ProcessDefinitions.RemoveRange(processDefinitionIds.Select(id => new ProcessDefinition() { Id = id }));
            return await this.processMiningDatabase.SaveChangesAsync(token);
        }
    }
}
