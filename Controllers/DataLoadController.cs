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
            //Token 확인
            response.Result = await RedisManager.Instance.TokenCheck(request.ID, request.AuthToken);
            if (response.Result == ErrorCode.Token_Fail_NotAuthorized)
            {
                return response;
            }

            //DB에서 체크
            var playerInfo = await MysqlManager.Instance.SelectGamePlayerQuery(request.ID);
            if (null == playerInfo)
            {
                response.Result = ErrorCode.Load_Fail_NotUser;
                return response;
            }
            
            Logger.ZLogInformation($"\nPlayer Data Load Completed!! \nID: {playerInfo.ID}" + 
                                   $"\nLevel: {playerInfo.Level}, \nExp: {playerInfo.Exp}, \nGameMoney: {playerInfo.GameMoney}");

            response.Player = playerInfo;
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
    public GamePlayer? Player { get; set; }
}


