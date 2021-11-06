using System.Text.RegularExpressions;

namespace Encoo.ProcessMining.DataContext.Model;

public record RegexMatchingOptions(string Pattern, RegexOptions[] Options);
