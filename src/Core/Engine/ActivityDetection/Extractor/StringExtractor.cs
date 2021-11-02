using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    class StringExtractor : IExtractor<string>
    {
        private readonly ITokenExtractorFactory tokenExtractorFactory = new DefaultTokenExtractorFactory();
        private readonly string template;
        private readonly ITokenExtractor[] tokenExtractors;

        public StringExtractor(ExtractionOptions options)
        {
            this.template = options.Template;
            this.tokenExtractors = options.Tokens
                .Select(t => this.tokenExtractorFactory.CreateTokenExtractor(t))
                .ToArray();
        }

        public string Extract(ContentData contentData, string[] matchingTokens) =>
            string.Format(
                this.template,
                this.tokenExtractors.Select(e => e.Extract(contentData, matchingTokens)).ToArray<object>());
    }
}
