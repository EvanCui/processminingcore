// See https://aka.ms/new-console-template for more information
using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.DataContext.Model;
using Encoo.ProcessMining.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

ConfigurationBuilder configurationBuilder = new();
configurationBuilder.AddJsonFile("appsettings.json");
configurationBuilder.AddEnvironmentVariables();
configurationBuilder.AddCommandLine(args);

var config = configurationBuilder.Build();

var builder = new DbContextOptionsBuilder<ProcessMiningDatabaseContext>();
builder.UseSqlServer(config.GetConnectionString("SqlServer"));

ProcessMiningDatabaseContext db = new ProcessMiningDatabaseContext(builder.Options);

var fileName = config["DataFile"];
Console.WriteLine("Loading file {0}?", fileName);
var key = Console.ReadKey();
if (key.Key != ConsoleKey.Y)
{
    Console.WriteLine("Exiting...");
    return;
}

Console.WriteLine("Loading from {0} ...", fileName);
var json = File.ReadAllText(fileName);
var dataItems = JsonConvert.DeserializeObject<List<DataItem>>(json);
var records = dataItems!.Select(d => new DataRecord
{
    Template = d.Template,
    ParametersArray = d.Parameters,
    Content = d.FormattedText,
    IsActivityDetected = false,
    IsTemplateDetected = true,
    IsDeleted = false,
    Time = d.Time
}).ToArray();

Console.WriteLine("Loaded {0} records, importing ...", records.Length);

int lastIndex = 0;
while (lastIndex < records.Length)
{
    var nextIndex = Math.Min(lastIndex + 10000, records.Length);
    db.DataRecords.AddRange(records[lastIndex..nextIndex]);
    lastIndex = nextIndex;

    await db.SaveChangesAsync(CancellationToken.None);
    db.ChangeTracker.Clear();

    Console.WriteLine("Total {0}, inserted {1}", records.Length, lastIndex);
}

Console.WriteLine("Done");
