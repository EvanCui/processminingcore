using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    interface IExtractorFactory
    {
        IExtractor<string> CreateSubjectExtractor(ExtractionOptions extractionOptions);
        IExtractor<string> CreateActorExtractor(ExtractionOptions extractionOptions);
        IExtractor<DateTimeOffset> CreateTimeExtractor(ExtractionOptions extractionOptions);
    }
}
