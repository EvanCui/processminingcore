using Encoo.ProcessMining.DataContext.Model;
using Encoo.ProcessMining.DataContext;

namespace Encoo.ProcessMining.Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Engine start! {0}", args.Length);

            var context = new ProcessMiningDatabaseContext();
            for (int i = 0; i < 10; i++)
            {
                context.ActivityDefinitions.Add(new ActivityDefinition() { Name = $"Definition {i}", Details = $"Details {i}" });
            }
            context.SaveChanges();
            var en = context.ActivityDefinitions.AsAsyncEnumerable();
            Print(en).Wait();
            //var engine = new ActivityDetectionEngine(new ActivityDetectionEngineOptions(), new )
        }

        static async Task Print(IAsyncEnumerable<ActivityDefinition> definitions)
        {
            await foreach (var definition in definitions)
            {
                Console.WriteLine(definition.Id);
            }
        }
    }
}
