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

//AddScoped: 단일 요청에서 공유하고 다른 요청에선 새 인스턴스 생성
builder.Services.AddScoped<IRealDbConnector, RealDbConnector>();

var app = builder.Build();

app.UseRouting();
app.UseLoggingMiddleware();
app.UseTokenCheckMiddleware();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

IConfiguration configuration = app.Configuration;

//객체 외부에서 생성, 주입
RealDbConnector conn = new RealDbConnector();
MysqlManager manager = new MysqlManager(configuration, conn);

RedisManager.Instance.Init(configuration);
CsvTableLoader.Instance.Load();

app.Run();
