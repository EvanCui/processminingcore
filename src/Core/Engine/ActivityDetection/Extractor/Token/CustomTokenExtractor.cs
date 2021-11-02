using Encoo.ProcessMining.DB.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    class CustomTokenExtractor : ITokenExtractor
    {
        private readonly JObject options;

        public CustomTokenExtractor(JObject options)
        {
            this.options = options;
        }

        public object Extract(ContentData contentData, string[] matchingTokens)
        {
            Debug.Assert(this.options != null);
            throw new NotImplementedException();
        }
    }
}
