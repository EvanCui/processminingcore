namespace Encoo.ProcessMining.DataContext.Model;

public record TemplateParametersMatchingOptions(
    string Template,
    Dictionary<int, string> Parameters);
