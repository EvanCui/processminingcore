using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Entities
{
    public record ProcessClusterStub(
        ProcessCluster Cluster,
        // thumbprint indexed
        IDictionary<string, ProcessDefinitionStub> ProcessDefinitions);
}
