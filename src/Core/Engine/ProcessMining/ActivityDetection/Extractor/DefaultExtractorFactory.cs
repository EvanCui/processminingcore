using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

class DefaultExtractorFactory : IExtractorFactory
{
    public IExtractor<string> CreateActorExtractor(ExtractionOptions extractionOptions) =>
        new StringExtractor(extractionOptions);

    public IExtractor<string> CreateSubjectExtractor(ExtractionOptions extractionOptions) =>
        new StringExtractor(extractionOptions);

    public IExtractor<DateTimeOffset?> CreateTimeExtractor(ExtractionOptions extractionOptions) =>
        new TimeExtractor(extractionOptions);
}
