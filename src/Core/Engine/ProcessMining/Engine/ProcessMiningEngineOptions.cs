namespace Encoo.ProcessMining.Engine;

public record ProcessMiningEngineOptions(int BatchLoadingSize = 1000, int MaxWorkItemSize = 10) : EngineOptions;
