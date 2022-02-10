namespace com2us_start.Middleware
{
    //미들웨어 등록
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCheckUserMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckUserMiddleware>();
        }
    }
}
