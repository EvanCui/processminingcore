using Encoo.ProcessMining.DataContext.Model;
using System.Text.RegularExpressions;

namespace Encoo.ProcessMining.Engine;

class RegexMatcher : IMatcher
{
    private readonly Regex regex;

    public RegexMatcher(RegexMatchingOptions options)
    {
        this.regex = new Regex(options.Pattern, options.Options.Aggregate((r, o) => r |= o));
    }

    public (bool Success, string[] Tokens) Match(ContentData contentData)
    {
        var matches = this.regex.Matches(contentData.Content);
        return (matches.Count > 0,
            matches.SelectMany(
                m => m.Groups.Values.Skip(1).SelectMany(
                    g => g.Captures.Select(c => c.Value))
                ).ToArray());
    }
}
