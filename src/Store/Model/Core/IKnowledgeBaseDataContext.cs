using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Core
{
    public interface IKnowledgeBaseDataContext
    {
        Task InitializeAsync(CancellationToken token);
        Task<ActivityDefinitionKnowledgeBase> GetKnowledgeBaseAsync(bool forceReload, CancellationToken token);
        Task AddActivityDefinitionAsync(ActivityDefinition definition, CancellationToken token);
        Task RemoveActivityDefinitionAsync(long activityDefinitionId, CancellationToken token);
    }
}
