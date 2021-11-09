using System;
using System.Collections.Generic;

#nullable disable

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ProcessInstance
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public bool IsClassified { get; set; }
        public string Thumbprint { get; set; }
        public long? ProcessClassificationId { get; set; }
    }
}
