using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    class TimeTokenExtractor : ITokenExtractor
    {
        public object Extract(ContentData contentData, string[] matchingTokens)
        {
            return contentData.Time;
        }
    }
}
