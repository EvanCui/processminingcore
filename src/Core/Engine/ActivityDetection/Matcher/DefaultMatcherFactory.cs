using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    class DefaultMatcherFactory : IMatcherFactory
    {
        public IMatcher CreateMatcher(MatchingOptions matchingOptions) =>
            matchingOptions.Type switch
            {
                MatchType.Keyword => new KeywordMatcher(matchingOptions.Keyword),
                MatchType.TemplateParameter => new TemplateParameterMatcher(matchingOptions.TemplateParameter),
                MatchType.Regex => new RegexMatcher(matchingOptions.Regex),
                MatchType.Custom => new CustomMatcher(matchingOptions.Custom),
                _ => throw new ArgumentException($"Unrecognized MatchType {matchingOptions.Type}", nameof(matchingOptions)),
            };
    }
}
