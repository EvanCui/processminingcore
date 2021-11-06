using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.DataContext.Model;
using Microsoft.AspNetCore.Mvc;

namespace Encoo.ProcessMining.Service.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class DataRecordsController : ControllerBase
    {
        private readonly ILogger<DataRecordsController> _logger;
        private readonly ProcessMiningDatabaseContext context;

        public DataRecordsController(ILogger<DataRecordsController> logger, ProcessMiningDatabaseContext context)
        {
            _logger = logger;
            this.context = context;
        }

        [HttpGet(Name = "GetRecords")]
        public IEnumerable<DataRecord> Get()
        {
            this._logger.LogInformation("Get called");
            return this.context.DataRecords.AsEnumerable();
        }
    }
}