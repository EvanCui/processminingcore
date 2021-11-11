using Microsoft.Extensions.DependencyInjection;

namespace Encoo.ProcessMining.Engine;

public static class InstantiationServices
{
    public static IServiceCollection AddInstantiationServices(this IServiceCollection serviceCollection) => serviceCollection
        .AddScoped<IProcessInstanceDetector, ProcessInstanceDetector>();
}
