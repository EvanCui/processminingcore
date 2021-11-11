using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

interface IMatcher
{
    (bool Success, string[] Tokens) Match(ContentData contentData);
}
