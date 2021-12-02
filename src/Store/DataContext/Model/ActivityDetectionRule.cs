using System;
using System.Collections.Generic;

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ActivityDetectionRule
    {
        public ActivityDetectionRule()
        {
            ActivityInstances = new HashSet<ActivityInstance>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string RuleData { get; set; }
        public long ActivityDefinitionId { get; set; }

        public virtual ActivityDefinition ActivityDefinition { get; set; }
        public virtual ICollection<ActivityInstance> ActivityInstances { get; set; }
    }
}
