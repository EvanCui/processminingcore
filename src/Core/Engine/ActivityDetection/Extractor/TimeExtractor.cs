using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

class TimeExtractor : IExtractor<DateTimeOffset?>
{
    private readonly StringExtractor stringExtractor;

    public TimeExtractor(ExtractionOptions options)
    {
        if (!(string.IsNullOrEmpty(options.Template)
            && options.Tokens.Length == 1
            && options.Tokens[0].Type == MatchType.Time))
        {
            this.stringExtractor = new StringExtractor(options);
        }
    }

    public DateTimeOffset? Extract(ContentData contentData, string[] matchingTokens)
    {
        if (this.stringExtractor == null)
        {
            return contentData.Time;
        }
        else
        {
            return DateTimeOffset.Parse(this.stringExtractor.Extract(contentData, matchingTokens));
        }
    }
}
