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
        Logger.ZLogInformation($"[Request Login] ID:{request.ID}, PW:{request.Password}");

        var response = new LoginResponse();
        response.Result = ErrorCode.None;

        try
        {
            var memberInfo = MysqlManager.Instance.SelectMemberQuery(request.ID);
            if (string.IsNullOrEmpty(memberInfo.Result.password))
            {
                response.Result = ErrorCode.Login_Fail_NotUser;
                return response;
            }

            var hashingPassword = CryptoManager.Instance.MakeHashingPassword(memberInfo.Result.salt, request.Password);
            if (memberInfo.Result.password != hashingPassword)
            {
                response.Result = ErrorCode.Login_Fail_Exception;
                return response;
            }
        }
        catch (Exception ex)
        {
            Logger.ZLogError(ex.ToString());
            response.Result = ErrorCode.Login_Fail_Exception;
            return response;
        }


        //유효기간 하루
        string tokenValue = RedisManager.Instance.AuthToken();
        var defaultExpiry = TimeSpan.FromDays(1);
        var redisId = new RedisString<string>(RedisManager.Instance.RedisConn, request.ID, defaultExpiry);
        await redisId.SetAsync(tokenValue);

        response.AuthToken = tokenValue;

        Logger.ZLogInformation($"Login Success!! Hi {request.ID}");
        return response;
    }
}


public class LoginRequest
{
    public string ID { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public ErrorCode Result { get; set; }
    public string AuthToken { get; set; }
}
