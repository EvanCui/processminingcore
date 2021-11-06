using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

class TemplateParameterMatcher : IMatcher
{
    private readonly TemplateParametersMatchingOptions options;

    public TemplateParameterMatcher(TemplateParametersMatchingOptions options)
    {
        this.options = options;
    }

    public (bool Success, string[] Tokens) Match(ContentData data)
    {
        bool success = string.Equals(data.Template, this.options.Template, StringComparison.OrdinalIgnoreCase);

        success = success && this.options.Parameters.All(
            p => data.Parameters.Length > p.Key && string.Equals(data.Parameters[p.Key], p.Value, StringComparison.OrdinalIgnoreCase));

        return (success, null);
    }
}
