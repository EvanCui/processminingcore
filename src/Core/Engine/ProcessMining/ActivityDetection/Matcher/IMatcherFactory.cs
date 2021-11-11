using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

interface IMatcherFactory
{
    IMatcher CreateMatcher(MatchingOptions matchingOptions);
}
