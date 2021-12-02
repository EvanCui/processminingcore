using Encoo.ProcessMining.DataContext;
using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.DataContext.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Encoo.ProcessMining.Service.Controllers
{
    [ApiController]
    [Route("v1/ProcessDefinitions/{processDefinitionId}/[controller]")]
    public class ActivityDefinitionsController : Controller
    {
        private readonly IKnowledgeBaseDataContext knowledgeBaseDataContext;
        private readonly ProcessMiningDatabaseContext databaseContext;

        public ActivityDefinitionsController(IKnowledgeBaseDataContext knowledgeBaseDataContext, ProcessMiningDatabaseContext databaseContext)
        {
            this.knowledgeBaseDataContext = knowledgeBaseDataContext;
            this.databaseContext = databaseContext;
        }

        // GET: ActivityDefinitionsController
        [HttpGet(Name = "GetActivityDefinitions")]
        public IEnumerable<ActivityDefinition> GetActivityDefinitionsAsync([FromRoute] long processDefinitionId, CancellationToken token)
        {
            return this.databaseContext.ActivityDefinitions
                .AsNoTracking()
                .Where(d => d.ProcessDefinitionId == processDefinitionId)
                .Include(d => d.ActivityDetectionRule);
        }

        [HttpGet("{id}", Name = "GetActivityDefinition")]

        public async Task<ActionResult> GetActivityDefinitionAsync([FromRoute] long processDefinitionId, [FromRoute] long id, CancellationToken cancellationToken)
        {
            var definition = await this.databaseContext.ActivityDefinitions.AsNoTracking()
                .Include(d => d.ActivityDetectionRule)
                .SingleOrDefaultAsync(d => d.Id == id && d.ProcessDefinitionId == processDefinitionId, cancellationToken);

            if (definition == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(definition);
            }
        }

        // POST: ActivityDefinitionsController
        [HttpPost(Name = "CreateActivityDefinition")]
        public async Task<IActionResult> CreateActivityDefinitionAsync([FromRoute] long processDefinitionId, [FromBody] ActivityDefinition activityDefinition, CancellationToken cancellationToken)
        {
            if (activityDefinition.ProcessDefinitionId == 0)
            {
                activityDefinition.ProcessDefinitionId = processDefinitionId;
            }

            if (activityDefinition.ProcessDefinitionId != processDefinitionId)
            {
                return BadRequest("processDefinitionId mismatch.");
            }

            var processDefinition = await this.databaseContext.ProcessDefinitions.SingleOrDefaultAsync(p => p.Id == processDefinitionId, cancellationToken);
            if (processDefinition == null)
            {
                return BadRequest("processDefinitionId not exists.");
            }

            return Ok(await this.knowledgeBaseDataContext.AddActivityDefinitionAsync(activityDefinition, cancellationToken));
        }

        // DELETE: ActivityDefinitionsController/5
        [HttpDelete("{id}", Name = "DeleteActivityDefinition")]
        public async Task<ActionResult> DeleteActivityDefinitionAsync([FromRoute] long processDefinitionId, int id, CancellationToken cancellationToken)
        {
            var activityDefinition = await this.databaseContext.ActivityDefinitions.SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
            if (activityDefinition == null)
            {
                return NotFound();
            }

            if (activityDefinition.ProcessDefinitionId != processDefinitionId)
            {
                return BadRequest("processDefinitionId mismatch.");
            }

            if (await this.knowledgeBaseDataContext.RemoveActivityDefinitionAsync(id, cancellationToken))
            {
                return new NoContentResult();
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}
