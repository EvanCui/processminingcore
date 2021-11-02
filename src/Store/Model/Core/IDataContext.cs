using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Core
{
    public interface IDataContext : IActivityDetectionResultSaver, IDataRecordLoader, IKnowledgeBaseDataContext
    {

    }
}
