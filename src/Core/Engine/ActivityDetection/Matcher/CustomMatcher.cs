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
    class CustomMatcher : IMatcher
    {
        private readonly JObject options;

        public CustomMatcher(JObject options)
        {
            this.options = options;
        }

        public (bool Success, string[] Tokens) Match(ContentData contentData)
        {
            Debug.Assert(this.options != null);
            throw new NotImplementedException();
        }
    }
}
