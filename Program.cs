using System.Collections.Immutable;
using ZLogger;
using com2us_start;
using com2us_start.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Logging.ClearProviders();
builder.Logging.AddZLoggerConsole();
//builder.Logging.AddConsole();

var app = builder.Build();

app.UseRouting();

//app.UseCheckUserMiddleware();
app.UseLoggingMiddleware();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

IConfiguration configuration = app.Configuration;
MysqlManager.Instance.Init((configuration));
RedisManager.Instance.Init(configuration);

app.Run();

