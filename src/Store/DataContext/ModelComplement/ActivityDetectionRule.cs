using Newtonsoft.Json;

namespace Encoo.ProcessMining.DataContext.Model;

public partial class ActivityDetectionRule
{
    private RuleOptions ruleOptions = null;
    public RuleOptions RuleOptions { get => this.ruleOptions ??= JsonConvert.DeserializeObject<RuleOptions>(this.RuleData); }
}
