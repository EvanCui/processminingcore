using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

class TimeTokenExtractor : ITokenExtractor
{
    public object Extract(ContentData contentData, string[] matchingTokens)
    {
        return contentData.Time;
    }
}
