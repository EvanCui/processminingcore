using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    interface IActivityDetectorsManager
    {
        void Initialize(IEnumerable<ActivityDefinition> definitions, bool rebuild = false);

        IActivityDetector GetDetector(long definitionId);
    }
}
