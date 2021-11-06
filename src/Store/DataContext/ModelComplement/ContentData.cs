namespace Encoo.ProcessMining.DataContext.Model;

public record ContentData(
    string Content,
    DateTimeOffset? Time,
    string Template,
    string[] Parameters);
