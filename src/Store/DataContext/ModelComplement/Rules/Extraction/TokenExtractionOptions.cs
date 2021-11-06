using Newtonsoft.Json.Linq;

namespace Encoo.ProcessMining.DataContext.Model;

public record TokenExtractionOptions(
    MatchType Type,
    KeywordExtractionOptions Keyword,
    TemplateParameterExtractionOptions TemplateParameter,
    RegexExtractionOptions Regex,
    JObject Custom);
