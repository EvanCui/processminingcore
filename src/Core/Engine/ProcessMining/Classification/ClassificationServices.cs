using Microsoft.Extensions.DependencyInjection;

namespace Encoo.ProcessMining.Engine;

public static class ClassificationServices
{
    public static IServiceCollection AddClassificationServices(this IServiceCollection serviceCollection) => serviceCollection
        .AddScoped<IProcessThumbprintIdentifier, ProcessThumbprintIdentifier>();
}
