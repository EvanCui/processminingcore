using Newtonsoft.Json.Linq;

namespace Encoo.ProcessMining.DataContext.Model;

public record MatchingOptions(
    MatchType Type,
    KeywordMatchingOptions Keyword,
    TemplateParametersMatchingOptions TemplateParameter,
    RegexMatchingOptions Regex,
    JObject Custom);
