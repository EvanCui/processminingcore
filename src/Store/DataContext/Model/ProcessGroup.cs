using System;
using System.Collections.Generic;

namespace Encoo.ProcessMining.DataContext.Model
{
    public partial class ProcessGroup
    {
        public long Id { get; set; }
        public string Thumbprint { get; set; }
        public string Name { get; set; }
        public bool IsAnalyzed { get; set; }
    }
}
