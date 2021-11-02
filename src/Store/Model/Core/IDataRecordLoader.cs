using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Core
{
    public interface IDataRecordLoader
    {
        IAsyncEnumerable<DataRecord> LoadDataRecordAsync(
            long currentKnowledgeWatermark,
            int batchSize,
            CancellationToken token);

        Task CompleteAsync(CancellationToken token);
        Task CancelAsync(CancellationToken token);
    }
}
