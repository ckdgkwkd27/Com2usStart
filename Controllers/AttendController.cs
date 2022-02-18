using CloudStructures.Structures;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class AttendController : ControllerBase
{
    private readonly ILogger Logger;

    public AttendController(ILogger<AttendController> logger)
    {
        Logger = logger;
    }

    [HttpPost]
    public async Task<AttendResponse> Post(AttendRequest request)
    {
        Logger.ZLogDebug($"[Request Join] ID:{request.ID}, Token:{request.AuthToken}");

        var response = new AttendResponse() { Result = ErrorCode.None };

        try
        {
            //Token 확인
            response.Result = await RedisManager.Instance.TokenCheck(request.ID, request.AuthToken);
            if (response.Result == ErrorCode.Token_Fail_NotAuthorized)
            {
                return response;
            }
            
            /*
             출석이 되어있는지 확인
             출석O => 기존 출석시간 업데이트
             출석X => 테이블에 유저정보 추가
            */
            var attendUser = await MysqlManager.Instance.SelectAttendQuery(request.ID);
            if (attendUser == null)
            {
                var cnt = await MysqlManager.Instance.InsertAttend(request.ID);
                if (cnt != 1)
                {
                    Logger.ZLogError("ERROR: Wrong User ID");
                    response.Result = ErrorCode.Attend_Fail_NotUser;
                    return response;
                }
            }
            else
            {
                var memberUpdateCount = await MysqlManager.Instance.UpdateAttend(request.ID);
                if (memberUpdateCount != 1)
                {
                    Logger.ZLogError("ERROR: Attend Update Failed!");
                    response.Result = ErrorCode.Attend_Fail_NotUser;
                    return response;
                }
            }

            return response;
        }
        catch (Exception ex)
        {
            Logger.ZLogError(ex.ToString());
            response.Result = ErrorCode.Attend_Fail_Exception;
            return response;
        }
    }
}

public class AttendRequest
{
    public string ID { get; set; }
    public string AuthToken { get; set; }
}

public class AttendResponse
{
    public ErrorCode Result { get; set; }
}
