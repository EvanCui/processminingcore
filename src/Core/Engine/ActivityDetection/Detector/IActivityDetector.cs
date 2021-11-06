using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

interface IActivityDetector
{
    ActivityDetectionResult Detect(DataRecord dataRecord);
}
