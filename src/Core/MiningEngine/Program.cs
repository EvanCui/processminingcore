using Encoo.ProcessMining.Service.HostedService;
using Encoo.ProcessMining.DataContext;
using Microsoft.EntityFrameworkCore;
using Encoo.ProcessMining.Engine;
using Encoo.ProcessMining.DataContext.DatabaseContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// data contexts
builder.Services.AddDbContext<ProcessMiningDatabaseContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddScoped<IKnowledgeBaseDataContext, KnowledgeBaseDataContext>();
builder.Services.AddScoped<IDataRecordDataContext, DataRecordDataContext>();
builder.Services.AddScoped<IActivityInstanceDataContext, ActivityInstanceDataContext>();

// hosted service
builder.Services.AddOptions<ActivityDetectionEngineOptions>();
builder.Services.AddScoped<ActivityDetectionEngine>();
builder.Services.AddHostedService<ActivityDetectionService>();

// controllers
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
