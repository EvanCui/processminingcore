using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

interface IExtractor<out T>
{
    T Extract(ContentData contentData, string[] matchingTokens);
}
