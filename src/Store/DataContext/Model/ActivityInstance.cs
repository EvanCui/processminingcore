using System;
using System.Collections.Generic;

#nullable disable

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ActivityInstance
    {
        public long Id { get; set; }
        public long DataRecordId { get; set; }
        public long ActivityDefinitionId { get; set; }
        public long DetectionRuleId { get; set; }
        public string ProcessSubject { get; set; }
        public DateTimeOffset? Time { get; set; }
        public string Actor { get; set; }
        public long? ProcessInstanceId { get; set; }

        public virtual ActivityDefinition ActivityDefinition { get; set; }
        public virtual DataRecord DataRecord { get; set; }
        public virtual ActivityDetectionRule DetectionRule { get; set; }
        public virtual ProcessInstance ProcessInstance { get; set; }
    }
}
