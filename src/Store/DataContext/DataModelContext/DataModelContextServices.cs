using Microsoft.Extensions.DependencyInjection;

namespace Encoo.ProcessMining.DataContext;

public static class DataModelContextServices
{
    public static IServiceCollection AddDataModelContextServices(this IServiceCollection serviceCollection) => serviceCollection
        .AddSingleton<IKnowledgeBaseDataContext, KnowledgeBaseDataContext>()
        .AddScoped<IActivityInstanceDataContext, ActivityInstanceDataContext>()
        .AddScoped<IDataRecordDataContext, DataRecordDataContext>()
        .AddScoped<IProcessInstanceDataContext, ProcessInstanceDataContext>();
}
