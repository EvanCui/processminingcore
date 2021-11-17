using Encoo.ProcessMining.DataContext;
using Encoo.ProcessMining.DataContext.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Encoo.ProcessMining.Service.Controllers
{
    public class ActivityDefinitionsController : Controller
    {
        private readonly IKnowledgeBaseDataContext knowledgeBaseDataContext;

        public ActivityDefinitionsController(IKnowledgeBaseDataContext knowledgeBaseDataContext)
        {
            this.knowledgeBaseDataContext = knowledgeBaseDataContext;
        }

        // GET: ActivityDefinitionsController
        [HttpGet(Name = "GetActivityDefinitions")]
        public async Task<IEnumerable<ActivityDefinition>> GetActivityDefinitionsAsync(CancellationToken token)
        {
            var knowledgeBase = await this.knowledgeBaseDataContext.GetKnowledgeBaseAsync(false, token);
            return knowledgeBase.FlattenedDefinitions;
        }

        // GET: ActivityDefinitionsController/5
        [HttpGet("{id}", Name = "GetActivityDefinition")]

        public async Task<ActionResult> GetActivityDefinitionAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var knowledgeBase = await this.knowledgeBaseDataContext.GetKnowledgeBaseAsync(false, cancellationToken);
            var definition = knowledgeBase.FlattenedDefinitions.FirstOrDefault(x => x.Id == id);
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
        public async Task<ActivityDefinition> CreateActivityDefinitionAsync([FromBody] ActivityDefinition activityDefinition, CancellationToken cancellationToken)
        {
            return await this.knowledgeBaseDataContext.AddActivityDefinitionAsync(activityDefinition, cancellationToken);
        }

        // DELETE: ActivityDefinitionsController/5
        [HttpDelete("{id}", Name = "DeleteActivityDefinition")]
        public async Task<ActionResult> DeleteActivityDefinitionAsync(int id, CancellationToken cancellationToken)
        {
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
