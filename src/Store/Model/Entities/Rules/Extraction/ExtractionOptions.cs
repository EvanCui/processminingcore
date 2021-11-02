namespace Encoo.ProcessMining.DB.Entities
{
    public record ExtractionOptions(
        string Template,
        TokenExtractionOptions[] Tokens);
}
