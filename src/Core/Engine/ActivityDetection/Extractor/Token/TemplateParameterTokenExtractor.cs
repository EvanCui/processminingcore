using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    class TemplateParameterTokenExtractor : ITokenExtractor
    {
        private readonly TemplateParameterExtractionOptions options;

        public TemplateParameterTokenExtractor(TemplateParameterExtractionOptions options)
        {
            this.options = options;
        }

        public object Extract(ContentData contentData, string[] matchingTokens)
        {
            // TODO: deal with the index overflow.
            return contentData.Parameters[this.options.Index];
        }
    }
}
