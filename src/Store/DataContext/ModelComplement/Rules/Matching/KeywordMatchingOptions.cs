namespace Encoo.ProcessMining.DataContext.Model;

public record KeywordMatchingOptions(
    string[] Keywords,
    KeywordOperator Operator);
