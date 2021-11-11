using Microsoft.Extensions.DependencyInjection;

namespace Encoo.ProcessMining.Engine;

public static class ActivityDetectionServices
{
    public static IServiceCollection AddActivityDetectionServices(this IServiceCollection serviceCollection) => serviceCollection
        .AddScoped<IActivityInstanceDetector, ActivityInstanceDetector>();
}
