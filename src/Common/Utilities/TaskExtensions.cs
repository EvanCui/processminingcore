namespace Encoo.ProcessMining.Utilities;

public static class TaskExtensions
{
    public static void FireAndForget(this Task t)
    {
        if (t is null)
        {
            throw new ArgumentNullException(nameof(t));
        }
    }
}
