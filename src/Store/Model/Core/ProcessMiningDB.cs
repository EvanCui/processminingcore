using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Core
{
    public class ProcessMiningDB : IProcessMiningDB
    {
        public DataStub Data => throw new NotImplementedException();

        public Task FlushAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task SaveDetectedActivityInstanceAsync(DataRecord dataRecord, ActivityInstance activityInstance, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
