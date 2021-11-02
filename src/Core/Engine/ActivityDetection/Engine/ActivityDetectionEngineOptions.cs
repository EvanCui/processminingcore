using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    public record ActivityDetectionEngineOptions(
        int BatchLoadingSize = 1000,
        int MaxWorkItemSize = 10);
}
