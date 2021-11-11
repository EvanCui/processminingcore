using Microsoft.Extensions.DependencyInjection;

namespace Encoo.ProcessMining.Engine;

public static class ClusterizationServices
{
    public static IServiceCollection AddClusterizationServices(this IServiceCollection serviceCollection) => serviceCollection
        .AddScoped<IProcessClusterDetector, ProcessClusterDetector>();
}
