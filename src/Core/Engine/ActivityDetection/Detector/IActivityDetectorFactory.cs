using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

interface IActivityDetectorFactory
{
    IActivityDetector CreateActivityDetector(ActivityDefinition definition);
}
