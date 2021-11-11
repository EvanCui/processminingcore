using Encoo.ProcessMining.DataContext;
using Microsoft.Extensions.DependencyInjection;

namespace Encoo.ProcessMining.Engine;

public static class ProcessMiningServices
{
    public static IServiceCollection AddProcessMiningServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions<ProcessMiningEngineOptions>();
        serviceCollection.AddOptions<EngineControllerOptions>();

        return serviceCollection
            .AddDataModelContextServices()
            .AddActivityDetectionServices()
            .AddInstantiationServices()
            .AddClassificationServices()
            .AddClusterizationServices()
            .AddProcessAnalyzerServices()
            .AddScoped<IEngine, ProcessMiningEngine>()
            .AddTransient<IEngineBuilder, ProcessMiningEngineBuilder>()
            .AddSingleton<IEngineController, EngineController<ProcessMiningEngineBuilder>>();
    }
}
