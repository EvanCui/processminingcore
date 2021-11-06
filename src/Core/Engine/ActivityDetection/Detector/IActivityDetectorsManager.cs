using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

interface IActivityDetectorsManager
{
    void Initialize(IEnumerable<ActivityDefinition> definitions, bool rebuild = false);

    IActivityDetector GetDetector(long definitionId);
}
