using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Core
{
    public interface IProcessMiningDB
    {
        Task SaveDetectedActivityInstanceAsync(
            DataRecord dataRecord,
            ActivityInstance activityInstance,
            CancellationToken token);

        Task FlushAsync(CancellationToken token);

        Task LoadAsync(CancellationToken token);

        DataStub Data { get; }
    }
}
