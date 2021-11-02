﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class DataRecord
    {
        public DataRecord()
        {
            ActivityInstances = new HashSet<ActivityInstance>();
        }

        public long Id { get; set; }
        public int Status { get; set; }
        public long? KnowledgeWatermark { get; set; }
        public string Content { get; set; }
        public DateTimeOffset? Time { get; set; }
        public string Template { get; set; }
        public string Parameters { get; set; }

        public virtual ICollection<ActivityInstance> ActivityInstances { get; set; }
    }
}
