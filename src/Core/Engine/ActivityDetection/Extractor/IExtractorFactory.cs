using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

interface IExtractorFactory
{
    IExtractor<string> CreateSubjectExtractor(ExtractionOptions extractionOptions);
    IExtractor<string> CreateActorExtractor(ExtractionOptions extractionOptions);
    IExtractor<DateTimeOffset?> CreateTimeExtractor(ExtractionOptions extractionOptions);
}
