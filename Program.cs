using System.Collections.Immutable;
using System.Configuration;
using ZLogger;
using com2us_start;
using com2us_start.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Logging.ClearProviders();
builder.Logging.AddZLoggerConsole();
//builder.Logging.AddConsole();

//AddScoped: 단일 요청에서 공유하고 다른 요청에선 새 인스턴스 생성
builder.Services.AddScoped<IRealDbConnector, RealDbConnector>();
builder.Services.AddScoped<IRealRedisConnector, RealRedisConnector>();


var app = builder.Build();

app.UseRouting();

//임시로 미들웨어 껐음
//app.UseLoggingMiddleware();
//app.UseTokenCheckMiddleware();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

IConfiguration configuration = app.Configuration;

await CsvTableLoader.Instance.Init(configuration);

app.Run();
