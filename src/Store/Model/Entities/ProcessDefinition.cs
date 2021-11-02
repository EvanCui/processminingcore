using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Entities
{
    public record ProcessDefinition(
        long? Id,
        string Name,
        string Thumbprint,
        int ProcessClusterId) : IntIdRecord(Id);
}
