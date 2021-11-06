namespace Encoo.ProcessMining.DataContext.Model;

public partial class ActivityInstance
{
    public ActivityInstance(
        string actor,
        DateTimeOffset? time,
        string processSubject,
        long detectionRuleId,
        long activityDefinitionId,
        long dataRecordId)
    {
        this.Actor = actor;
        this.Time = time;
        this.ProcessSubject = processSubject;
        this.DetectionRuleId = detectionRuleId;
        this.ActivityDefinitionId = activityDefinitionId;
        this.DataRecordId = dataRecordId;
    }
}
