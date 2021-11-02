using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Core
{
    class KnowledgeBaseDataContext : IKnowledgeBaseDataContext
    {
        private bool shouldReload = true;
        private readonly IDbConnection dbConnection;

        public KnowledgeBaseDataContext(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public Task AddActivityDefinitionAsync(ActivityDefinition definition, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task RemoveActivityDefinitionAsync(long activityDefinitionId, CancellationToken token)
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
    }
}
