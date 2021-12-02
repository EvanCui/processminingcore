using System;
using System.Collections.Generic;

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ProcessInstance
    {
        public ProcessInstance()
        {
            ActivityInstances = new HashSet<ActivityInstance>();
        }

        public long Id { get; set; }
        public string Subject { get; set; }
        public bool IsClustered { get; set; }
        public bool IsAnalyzed { get; set; }
        public string Thumbprint { get; set; }
        public long? ProcessClusterId { get; set; }

        public virtual ProcessCluster ProcessCluster { get; set; }
        public virtual ICollection<ActivityInstance> ActivityInstances { get; set; }
    }
}
