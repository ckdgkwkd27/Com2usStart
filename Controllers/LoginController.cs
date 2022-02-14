using CloudStructures.Structures;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILogger Logger;
    private IMemoryCache MemoryCache;

    public LoginController(ILogger<LoginController> logger, IMemoryCache memoryCache)
    {
        Logger = logger;
        MemoryCache = memoryCache; 
    }

    [HttpPost]
    public async Task<LoginResponse> Post(LoginRequest request)
    {
        //ZLogger 적용
        Logger.ZLogInformation($"[Request Login] ID:{request._ID}, PW:{request._Password}");
        Logger.ZLogDebug($"[Request Login] ID:{request._ID}, PW:{request._Password}");
        
        var response = new LoginResponse();
        response.Result = ErrorCode.None;
        
        using (var conn = await DBManager.Instance.GetDBConnection())
        {
            var memberInfo = await conn.QuerySingleOrDefaultAsync<Member>(
                @"select ID, Password, Salt from com2us.account where ID=@id", 
                new { id = request._ID });

            if (memberInfo == null || string.IsNullOrEmpty(memberInfo.password))
            {
                response.Result = ErrorCode.Login_Fail_NotUser;
                return response;
            }

            var hashingPassword = DBManager.Instance.MakeHashingPassword(memberInfo.salt, request._Password);
            if (memberInfo.password != hashingPassword)
            {
                response.Result = ErrorCode.Login_Fail_Exception;
                return response;
            }
        }
        
        //유효기간 하루
        string tokenValue = DBManager.Instance.AuthToken();
        var defaultExpiry = TimeSpan.FromDays(1);
        var redisId = new RedisString<string>(DBManager.Instance.RedisConn, request._ID, defaultExpiry);
        await redisId.SetAsync(tokenValue);

        response.AuthToken = tokenValue;
        
        if (response.Result == ErrorCode.None)
        {
            Logger.ZLogInformation("Login Success!! Hi", request._ID);
        }
        return response;
    }
}


public class LoginRequest
{
    public string _ID { get; set; }
    public string _Password { get; set; }
}

public class LoginResponse
{
    public ErrorCode Result { get; set; }
    public string AuthToken { get; set; }
}
