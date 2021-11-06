namespace Encoo.ProcessMining.DataContext.Model;

public record ExtractionOptions(
    string Template,
    TokenExtractionOptions[] Tokens);
