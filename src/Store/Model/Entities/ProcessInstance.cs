using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Entities
{
    public record ProcessInstance(
        long? Id,
        string Subject,
        SortedDictionary<DateTimeOffset, long> ActivityDefinitionIds,
        long? DefinitionId) : IntIdRecord(Id);
}
