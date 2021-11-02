using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    class RegexMatcher : IMatcher
    {
        private readonly Regex regex;

        public RegexMatcher(RegexMatchingOptions options)
        {
            this.regex = new Regex(options.Expression, options.Options.Aggregate((r, o) => r |= o));
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
}
