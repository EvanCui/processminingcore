using System;
using System.Collections.Generic;

#nullable disable

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ProcessGroup
    {
        public ProcessGroup()
        {
            ProcessInstances = new HashSet<ProcessInstance>();
        }

        public long Id { get; set; }
        public string Thumbprint { get; set; }
        public string Name { get; set; }
        public bool IsAnalyzed { get; set; }

        public virtual ICollection<ProcessInstance> ProcessInstances { get; set; }
    }
}
