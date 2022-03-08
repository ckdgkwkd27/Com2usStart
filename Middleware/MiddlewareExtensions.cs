namespace com2us_start.Middleware
{
    //미들웨어 등록
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }

        public static IApplicationBuilder UseTokenCheckMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthCheckMiddleware>();
        }
    }
}
