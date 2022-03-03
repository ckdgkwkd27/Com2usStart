using System.Text;
using ZLogger;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace com2us_start.Middleware;

public class TokenCheckMiddleware
{
    private readonly RequestDelegate _requestDelegate;
    private readonly ILogger _logger;

    public TokenCheckMiddleware(RequestDelegate requestDelegate, ILogger<TokenCheckMiddleware> logger)
    {
        _requestDelegate = requestDelegate;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Path != "/Join"
            && httpContext.Request.Path != "/Login")
        {
            StreamReader bodystream = new StreamReader(httpContext.Request.Body, Encoding.Default);

            var body = await bodystream.ReadToEndAsync();
            var obj = JsonSerializer.Deserialize<UserInfo>(body);

            var id = obj?.Id;
            var token = obj?.AuthToken;
            
            _logger.ZLogInformation($"ID: {id}, Token: {token} in TokenCheckMiddleware");
            
            //Token 확인
            ErrorCode result = await RedisManager._realRedisConnector.TokenCheck(id, token);
            if (result == ErrorCode.Token_Fail_NotAuthorized)
            {
                return;
            }

            httpContext.Request.Body = new MemoryStream(Encoding.Default.GetBytes(body));
        }
        await _requestDelegate(httpContext); 
    }
}

class UserInfo
{
    public string? Id { get; set; }
    public string? AuthToken { get; set; }
}