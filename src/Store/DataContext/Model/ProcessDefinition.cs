using System;
using System.Collections.Generic;

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ProcessDefinition
    {
        public ProcessDefinition()
        {
            ActivityDefinitions = new HashSet<ActivityDefinition>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }

        public virtual ICollection<ActivityDefinition> ActivityDefinitions { get; set; }
    }
}
