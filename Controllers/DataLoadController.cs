using CloudStructures.Structures;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class DataLoadController : ControllerBase
{
    private readonly ILogger Logger;

    public DataLoadController(ILogger<DataLoadController> logger)
    {
        Logger = logger;
    }

    [HttpPost]
    public async Task<LoadResponse> Post(LoadRequest request)
    {
        Logger.ZLogDebug($"[Request Join] ID:{request.ID}, Token:{request.AuthToken}");

        var response = new LoadResponse() { Result = ErrorCode.None };

        try
        {
            var playerInfo = MysqlManager.Instance.SelectGamePlayerQuery(request.ID);
            if (string.IsNullOrEmpty(playerInfo.Result.ID))
            {
                response.Result = ErrorCode.Load_Fail_NotUser;
                return response;
            }

            //Redis에서 체크
            var defaultExpiry = TimeSpan.FromDays(1);
            var redisId = new RedisString<string>(RedisManager.Instance.RedisConn, request.ID, defaultExpiry);
            if (redisId.GetAsync().Result.Value != request.AuthToken)
            {
                response.Result = ErrorCode.Load_Fail_NotAuthorized;
                return response;
            }
            
            Logger.ZLogInformation($"\nPlayer Data Load Completed!! \nID: {playerInfo.Result.ID}" + 
                                   $"\nLevel: {playerInfo.Result.Level}, \nExp: {playerInfo.Result.Exp}, \nGameMoney: {playerInfo.Result.GameMoney}");

            var playerRobotmon = MysqlManager.Instance.SelectPlayerRobotmonQuery(request.ID);
            if (playerRobotmon.IsCompletedSuccessfully)
            {
                Logger.ZLogInformation("보유한 로봇몬: ");
                
            } 
        }
        catch (Exception ex)
        {
            Logger.ZLogError(ex.ToString());
            response.Result = ErrorCode.Load_Fail_Exception;
            return response;
        }

        return response;
    }
}

public class LoadRequest
{
    public string ID { get; set; }
    public string AuthToken { get; set; }
}

public class LoadResponse
{
    public ErrorCode Result { get; set; }
}


