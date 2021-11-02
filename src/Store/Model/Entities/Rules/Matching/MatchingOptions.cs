using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Encoo.ProcessMining.DB.Entities
{
    public record MatchingOptions(
        MatchType Type,
        KeywordMatchingOptions Keyword,
        TemplateParametersMatchingOptions TemplateParameter,
        RegexMatchingOptions Regex,
        JObject Custom);
}
