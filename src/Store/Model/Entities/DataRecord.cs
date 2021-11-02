using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Entities
{
    public record DataRecord(
        long? Id,
        ContentData Data,
        long KnowledgeWatermark,
        DataRecordStatus Status) : IntIdRecord(Id);
}
