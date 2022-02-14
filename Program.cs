using System.Collections.Immutable;
using ZLogger;
using com2us_start;
using com2us_start.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddMvcOptions(options => options.Filters.Add(typeof(ResultFilterChangeResponse)));
builder.Services.AddMemoryCache();

builder.Logging.ClearProviders();
builder.Logging.AddZLoggerConsole();
//builder.Logging.AddConsole();

var app = builder.Build();

if (false == app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

//app.UseCheckUserMiddleware();
app.UseLoggingMiddleware();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

IConfiguration configuration = app.Configuration;
DBManager.Instance.Init(configuration);

app.Run();

