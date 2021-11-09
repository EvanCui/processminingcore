namespace Encoo.ProcessMining.Engine;

public record ActivityDetectionEngineOptions(int BatchLoadingSize = 1000, int MaxWorkItemSize = 10) : EngineOptions;
