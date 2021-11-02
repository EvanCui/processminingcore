using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Entities
{
    public record ActivityInstance(
        long? Id,
        string Actor,
        DateTimeOffset Time,
        string ProcessSubject,
        long DetectionRuleId,
        long ActivityDefinitionId,
        long DataRecordId,
        long? ProcessInstanceId) : IntIdRecord(Id);
}
