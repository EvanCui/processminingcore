using System;
using System.Collections.Generic;

#nullable disable

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ActivityDefinition
    {
        public ActivityDefinition()
        {
            ActivityInstances = new HashSet<ActivityInstance>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }

        public virtual ActivityDetectionRule ActivityDetectionRule { get; set; }
        public virtual ICollection<ActivityInstance> ActivityInstances { get; set; }
    }
}
