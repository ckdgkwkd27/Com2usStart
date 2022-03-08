using System.Text;
using Microsoft.AspNetCore.Identity;
using ZLogger;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace com2us_start.Middleware;

public class AuthCheckMiddleware
{
    private readonly RequestDelegate _requestDelegate;
    private readonly ILogger _logger;

    public AuthCheckMiddleware(RequestDelegate requestDelegate, ILogger<AuthCheckMiddleware> logger)
    {
        _requestDelegate = requestDelegate;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext, IRealDbConnector realDbConnector)
    {
        if (httpContext.Request.Path != "/Join"
            && httpContext.Request.Path != "/Login")
        {
            StreamReader bodystream = new StreamReader(httpContext.Request.Body, Encoding.Default);
            
            var body = await bodystream.ReadToEndAsync();
            var obj = JsonSerializer.Deserialize<UserInfo>(body);

            var id = obj?.ID;
            var playerId = obj?.PlayerID;
            var token = obj?.AuthToken;

            /*if (playerId != null)
            {
                var player = await realDbConnector.SelectGamePlayer(playerId);
                if (player == null)
                {
                    _logger.ZLogError("ERROR: Wrong PlayerID!");
                    return;
                }
            }*/
            _logger.ZLogInformation($"ID: {id}, Token: {token} in TokenCheckMiddleware");
            
            //Token 확인
            ErrorCode result = await RealRedisConnector.TokenCheck(id, token);
            if (result == ErrorCode.Token_Fail_NotAuthorized)
            {
                _logger.ZLogError($"ERROR: Not Authorized Token");
                return;
            }

            httpContext.Request.Body = new MemoryStream(Encoding.Default.GetBytes(body));
        }
        await _requestDelegate(httpContext);
    }
}

class UserInfo
{
    public string? ID { get; set; }
    public string? PlayerID { get; set; }
    public string? AuthToken { get; set; }
}