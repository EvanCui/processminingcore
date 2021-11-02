using System;
using System.Collections.Generic;

#nullable disable

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ActivityDefinition
    {
        public ActivityDefinition()
        {
            ActivityDetectionRules = new HashSet<ActivityDetectionRule>();
            ActivityInstances = new HashSet<ActivityInstance>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }

        public virtual ICollection<ActivityDetectionRule> ActivityDetectionRules { get; set; }
        public virtual ICollection<ActivityInstance> ActivityInstances { get; set; }
    }
}
