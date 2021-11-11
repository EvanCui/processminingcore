namespace Encoo.ProcessMining.Tools;

public record DataItem(string FormattedText, string Template, int TemplateId, string[] Parameters, DateTimeOffset Time);
