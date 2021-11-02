using Newtonsoft.Json.Linq;

namespace Encoo.ProcessMining.DB.Entities
{
    public record TokenExtractionOptions(
        MatchType Type,
        KeywordExtractionOptions Keyword,
        TemplateParameterExtractionOptions TemplateParameter,
        RegexExtractionOptions Regex,
        JObject Custom);
}
