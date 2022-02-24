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
    private readonly ILogger _logger;
    private readonly IConfiguration _conf;
    private readonly IRealDbConnector _realDbConnector;
    private IMemoryCache MemoryCache;

    public LoginController(ILogger<LoginController> logger, IConfiguration conf, IRealDbConnector realDbConnector, IMemoryCache memoryCache)
    {
        _logger = logger;
        _conf = conf;
        _realDbConnector = realDbConnector;
        MemoryCache = memoryCache; 
    }

    [HttpPost]
    public async Task<LoginResponse> Post(LoginRequest request)
    {
        //ZLogger 적용
        _logger.ZLogInformation($"[Request Login] ID:{request.ID}, PW:{request.Password}");

        var response = new LoginResponse() { Result = ErrorCode.None };
        
        try
        {
            using MysqlManager manager = new MysqlManager(_conf, _realDbConnector);
            await manager.GetDbConnection();
            
            var memberInfo = await manager.SelectMemberQuery(request.ID);
            if (null == memberInfo)
            {
                response.Result = ErrorCode.Login_Fail_NotUser;
                return response;
            }

            var hashingPassword = CryptoManager.Instance.MakeHashingPassword(memberInfo.salt, request.Password);
            if (memberInfo.password != hashingPassword)
            {
                response.Result = ErrorCode.Login_Fail_Exception;
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex.ToString());
            response.Result = ErrorCode.Login_Fail_Exception;
            return response;
        }
        
        //유효기간 하루
        string tokenValue = RedisManager.Instance.AuthToken();
        var defaultExpiry = TimeSpan.FromDays(1);
        var redisId = new RedisString<string>(RedisManager.Instance.RedisConn, request.ID, defaultExpiry);
        await redisId.SetAsync(tokenValue);

        response.AuthToken = tokenValue;

        _logger.ZLogInformation($"Login Success!! Hi {request.ID}");
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
