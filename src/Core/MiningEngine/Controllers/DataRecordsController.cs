using Encoo.ProcessMining.DataContext;
using Encoo.ProcessMining.DataContext.Model;
using Encoo.ProcessMining.Service.Model;
using Microsoft.AspNetCore.Mvc;

namespace Encoo.ProcessMining.Service.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class DataRecordsController : ControllerBase
    {
        private readonly ILogger<DataRecordsController> _logger;
        private readonly IDataRecordDataContext context;
        private readonly IKnowledgeBaseDataContext knowledgeBaseDataContext;

        public DataRecordsController(ILogger<DataRecordsController> logger, IDataRecordDataContext context, IKnowledgeBaseDataContext knowledgeBaseDataContext)
        {
            _logger = logger;
            this.context = context;
            this.knowledgeBaseDataContext = knowledgeBaseDataContext;
        }

        [HttpGet(Name = "GetRecords")]
        public async Task<IAsyncEnumerable<DataRecord>> GetRecords(CancellationToken token, [FromQuery] string purpose = "Investigate", [FromQuery] int batchSize = 100)
        {
            this._logger.LogInformation("Get record called, purpose = {purpose}, batchSize = {batchSize}", purpose, batchSize);

            if (!Enum.TryParse(purpose, true, out GetRecordPurpose thePurpose))
            {
                this._logger.LogError("Non-supported purpose {purpose}", purpose);
                throw new NotSupportedException($"Non-supported purpose {purpose}");
            }

            long currentKnowledgeWatermark = await this.knowledgeBaseDataContext.GetKnowledgeWatermarkAsync(token);
            return thePurpose switch
            {
                GetRecordPurpose.Investigate => this.context.LoadDataRecordToInvestigateAsync(currentKnowledgeWatermark, batchSize, token),
                _ => throw new NotImplementedException($"Not implemented purpose {thePurpose}"),
            };
        }

        [HttpDelete(Name = "DeleteRecords")]
        public Task DeleteRecords(CancellationToken token, [FromBody] long[]? recordIds = null)
        {
            return this.context.DeleteDataRecordsAsync(recordIds, token);
        }
    }
}