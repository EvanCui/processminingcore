using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Core
{
    class DataContext : IDataContext
    {
        public Task AddActivityDefinitionAsync(ActivityDefinition definition, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task CancelAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task CompleteAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<ActivityDefinitionKnowledgeBase> GetKnowledgeBaseAsync(bool forceReload, CancellationToken token)
        {

            throw new NotImplementedException();
        }

        public Task InitializeAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<DataRecord> LoadDataRecordAsync(long currentKnowledgeWatermark, int batchSize, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task RemoveActivityDefinitionAsync(long activityDefinitionId, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task SaveDetectionResultAsync(IEnumerable<ActivityDetectionResult> results, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
