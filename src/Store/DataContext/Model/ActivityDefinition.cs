using System;
using System.Collections.Generic;

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ActivityDefinition
    {
        public ActivityDefinition()
        {
            ActivityInstances = new HashSet<ActivityInstance>();
            ProcessClusterActivityDefinitionRelationships = new HashSet<ProcessClusterActivityDefinitionRelationship>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public long ProcessDefinitionId { get; set; }

        public virtual ProcessDefinition ProcessDefinition { get; set; }
        public virtual ActivityDetectionRule ActivityDetectionRule { get; set; }
        public virtual ICollection<ActivityInstance> ActivityInstances { get; set; }
        public virtual ICollection<ProcessClusterActivityDefinitionRelationship> ProcessClusterActivityDefinitionRelationships { get; set; }
    }
}
