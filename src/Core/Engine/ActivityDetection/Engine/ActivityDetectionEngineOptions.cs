namespace Encoo.ProcessMining.Engine;

public class ActivityDetectionEngineOptions
{
    public int BatchLoadingSize { get; set; } = 1000;
    public int MaxWorkItemSize { get; set; } = 10;
    public int DetectionIntervalSeconds { get; set; } = 60;
    public int ErrorRetryIntervalSeconds { get; set; } = 5;

}
