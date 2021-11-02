using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
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
}
