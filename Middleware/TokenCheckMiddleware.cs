using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZLogger;

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
            //await도 고려
            var body = bodystream.ReadToEndAsync().Result;
            var obj = (JObject)JsonConvert.DeserializeObject(body);
            
            var Id = (string)obj["ID"];
            var token = (string)obj["AuthToken"];
            
            _logger.ZLogInformation($"ID: {Id}, Token: {token} in TokenCheckMiddleware");
            
            //Token 확인
            ErrorCode result = await RedisManager.Instance.TokenCheck(Id, token);
            if (result == ErrorCode.Token_Fail_NotAuthorized)
            {
                return;
            }

            httpContext.Request.Body = new MemoryStream(Encoding.Default.GetBytes(body));
        }
        await _requestDelegate(httpContext); 
    }
}
