namespace com2us_start.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _requestDelegate;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate requestDelegate, ILogger<LoggingMiddleware> logger)
    {
        _requestDelegate = requestDelegate;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        _logger.LogInformation("처리 시작: " + httpContext.Request.Path);
        await _requestDelegate(httpContext);
        _logger.LogInformation("처리 종료");
    }
}

public static class LoggingMiddlewareExtensions
{
    /*public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }*/
}