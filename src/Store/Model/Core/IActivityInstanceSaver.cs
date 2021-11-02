using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Core
{
    public interface IActivityDetectionResultSaver
    {
        Task SaveDetectionResultAsync(IEnumerable<ActivityDetectionResult> results, CancellationToken token);
    }
}
