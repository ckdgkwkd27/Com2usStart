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

        var memberInfo = MysqlManager.Instance.SelectMemberQuery(request.ID);

        if (string.IsNullOrEmpty(memberInfo.Result.password))
        {
            response.Result = ErrorCode.Login_Fail_NotUser;
            return response;
        }

        var hashingPassword = MysqlManager.Instance.MakeHashingPassword(memberInfo.Result.salt, request.Password);
        if (memberInfo.Result.password != hashingPassword)
        {
            response.Result = ErrorCode.Login_Fail_Exception;
            return response;
        }
        
        
        //유효기간 하루
        string tokenValue = RedisManager.Instance.AuthToken();
        var defaultExpiry = TimeSpan.FromDays(1);
        var redisId = new RedisString<string>(RedisManager.Instance.RedisConn, request.ID, defaultExpiry);
        await redisId.SetAsync(tokenValue);

        response.AuthToken = tokenValue;
        
        if (response.Result == ErrorCode.None)
        {
            Logger.ZLogInformation($"Login Success!! Hi {request.ID}");
            
            //Player Data 로딩
            var player = MysqlManager.Instance.SelectGamePlayerQuery(request.ID);
            if (string.IsNullOrEmpty(player.Result.ID))
            {
                Logger.ZLogError("ERROR: Player Loading Failed!");
                response.Result = ErrorCode.Login_Fail_NoPlayerData;
                return response;
            }
            
            Logger.ZLogInformation($"\nPlayer Data Load Completed!! \nID: {player.Result.ID}" + 
                                   $"\nLevel: {player.Result.Level}, \nExp: {player.Result.Exp}, \nGameMoney: {player.Result.GameMoney}");

            var playerRobotmon = MysqlManager.Instance.SelectPlayerRobotmonQuery(request.ID);
            if (playerRobotmon.IsCompletedSuccessfully)
            {
                Logger.ZLogInformation("보유한 로봇몬: ");
                
            } 
        }
        
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
