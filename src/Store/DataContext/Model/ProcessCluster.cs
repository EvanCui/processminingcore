using System;
using System.Collections.Generic;

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ProcessCluster
    {
        public ProcessCluster()
        {
            ProcessInstances = new HashSet<ProcessInstance>();
        }

        public long Id { get; set; }
        public string Thumbprint { get; set; }
        public string Name { get; set; }
        public bool IsAnalyzed { get; set; }

        public virtual ProcessClusterActivityDefinitionRelationship ProcessClusterActivityDefinitionRelationship { get; set; }
        public virtual ICollection<ProcessInstance> ProcessInstances { get; set; }
    }
}
