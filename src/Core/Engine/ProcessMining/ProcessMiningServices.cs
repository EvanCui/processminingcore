using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
    public static class ProcessMiningServices
    {
        public static IServiceCollection AddProcessMiningServices(this IServiceCollection serviceCollection) =>
            serviceCollection.AddSingleton<IEngine, ProcessMiningEngine>()
                .AddSingleton<IProcessInstanceDetector, ProcessInstanceDetector>();
    }
}
