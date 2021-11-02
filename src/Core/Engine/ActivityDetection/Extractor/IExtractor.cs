using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    interface IExtractor<out T>
    {
        T Extract(ContentData contentData, string[] matchingTokens);
    }
}
