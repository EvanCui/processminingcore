using System;
using System.Collections.Generic;

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ProcessClusterActivityDefinitionRelationship
    {
        public long ProcessClusterId { get; set; }
        public long ActivityDefinitionId { get; set; }

        public virtual ActivityDefinition ActivityDefinition { get; set; }
        public virtual ProcessCluster ProcessCluster { get; set; }
    }
}
