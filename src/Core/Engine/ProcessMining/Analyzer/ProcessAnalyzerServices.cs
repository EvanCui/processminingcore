using Microsoft.Extensions.DependencyInjection;

namespace Encoo.ProcessMining.Engine;

public static class ProcessAnalyzerServices
{
    public static IServiceCollection AddProcessAnalyzerServices(this IServiceCollection serviceCollection) => serviceCollection
        .AddScoped<IProcessAnalyzer, ProcessAnalyzer>();
}
