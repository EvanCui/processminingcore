using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    class DefaultTokenExtractorFactory : ITokenExtractorFactory
    {
        public ITokenExtractor CreateTokenExtractor(TokenExtractionOptions options) =>
            options.Type switch
            {
                MatchType.Keyword => new KeywordTokenExtractor(options.Keyword),
                MatchType.TemplateParameter => new TemplateParameterTokenExtractor(options.TemplateParameter),
                MatchType.Regex => new RegexTokenExtractor(options.Regex),
                MatchType.Custom => new CustomTokenExtractor(options.Custom),
                MatchType.Time => new TimeTokenExtractor(),
                var t => throw new ArgumentException($"Unsupported token extraction type {t}", nameof(options)),
            };
    }
}
