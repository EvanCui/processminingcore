using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Entities
{
    public record ActivityDefinition(
        long? Id,
        string Name,
        string Details,
        ActivityDetectionRule Rule) : IntIdRecord(Id);
}
