using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Core
{
    public record ActivityDefinitionKnowledgeBase(
        long Watermark,
        List<IGrouping<int, ActivityDefinition>> PrioritizedDefinitions)
    {
        public IEnumerable<ActivityDefinition> FlattenedDefinitions => this.PrioritizedDefinitions.SelectMany(g => g);
    }
}
