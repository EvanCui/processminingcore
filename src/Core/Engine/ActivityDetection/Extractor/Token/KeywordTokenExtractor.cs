using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    class KeywordTokenExtractor : ITokenExtractor
    {
        private readonly KeywordExtractionOptions options;

        public KeywordTokenExtractor(KeywordExtractionOptions options)
        {
            this.options = options;
        }

        public object Extract(ContentData contentData, string[] matchingTokens)
        {
            int start = this.options.Start switch
            {
                null or "" => 0,
                var s => contentData.Content.IndexOf(s),
            };

            if (start == -1) { return null; }

            int end = this.options.End switch
            {
                null or "" => contentData.Content.Length,
                var s => contentData.Content.IndexOf(s),
            };

            if (end <= start) { return null; }

            return contentData.Content[start..end];
        }
    }
}
