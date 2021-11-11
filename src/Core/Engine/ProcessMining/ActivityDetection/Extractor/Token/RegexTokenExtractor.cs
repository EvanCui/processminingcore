using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

class RegexTokenExtractor : ITokenExtractor
{
    private readonly RegexExtractionOptions options;

    public RegexTokenExtractor(RegexExtractionOptions options)
    {
        this.options = options;
    }

    public object Extract(ContentData contentData, string[] matchingTokens)
    {
        // TODO: deal with the index overflow.
        return matchingTokens[this.options.Index];
    }
}
