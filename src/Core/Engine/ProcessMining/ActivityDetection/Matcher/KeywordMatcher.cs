using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

class KeywordMatcher : IMatcher
{
    private readonly KeywordMatchingOptions options;

    public KeywordMatcher(KeywordMatchingOptions options)
    {
        this.options = options;
    }

    public (bool Success, string[] Tokens) Match(ContentData contentData) =>
        this.options.Operator switch
        {
            KeywordOperator.And => (this.options.Keywords.All(keyword => contentData.Content.Contains(keyword)), null),
            KeywordOperator.Or => (this.options.Keywords.Any(keyword => contentData.Content.Contains(keyword)), null),
            _ => (false, null),
        };
}
