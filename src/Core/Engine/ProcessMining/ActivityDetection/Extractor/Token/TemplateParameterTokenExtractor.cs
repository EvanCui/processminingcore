using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

class TemplateParameterTokenExtractor : ITokenExtractor
{
    private readonly TemplateParameterExtractionOptions options;

    public TemplateParameterTokenExtractor(TemplateParameterExtractionOptions options)
    {
        this.options = options;
    }

    public object Extract(ContentData contentData, string[] matchingTokens)
    {
        // TODO: deal with the index overflow.
        return contentData.Parameters[this.options.Index];
    }
}
