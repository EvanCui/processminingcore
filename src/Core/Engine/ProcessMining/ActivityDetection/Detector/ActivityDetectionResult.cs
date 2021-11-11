using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

public record ActivityDetectionResult(
    DataRecord DataRecord,
    ActivityInstance ActivityInstance);
