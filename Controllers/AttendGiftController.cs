using com2us_start.TableImpl;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class AttendGiftController : ControllerBase
{
    private readonly ILogger Logger;
    private Int32 TestMinutesLimit;

    public AttendGiftController(ILogger<AttendGiftController> logger)
    {
        Logger = logger;
        TestMinutesLimit = 1;
    }

    [HttpPost]
    public async Task<AttendResponse> Post(AttendRequest request)
    {
        Logger.ZLogDebug($"[Request] ID:{request.ID}, Token:{request.AuthToken}");

        var response = new AttendResponse() { Result = ErrorCode.None };

        try
        {
            response.Result = await RedisManager.Instance.TokenCheck(request.ID, request.AuthToken);
            if (response.Result == ErrorCode.Token_Fail_NotAuthorized)
            {
                return response;
            }

            //Attendance 테이블 조회해서 GiftDate가 null이거나 Time Limit을 넘었으면 선물 지급
            var attendGiftInfo = await MysqlManager.Instance.SelectAttendQuery(request.ID);
            if (attendGiftInfo == null)
            {
                Logger.ZLogError("Wrong User ID");
                response.Result = ErrorCode.Attend_Fail_NotUser;
                return response;
            }
            
            TimeSpan elapsed = DateTime.Now - attendGiftInfo.GiftDate;

            //현재 테스트를 위해 1분단위로 출석 가능
            if (elapsed.Minutes > TestMinutesLimit)
            {
                //출석보상 편지를 우편함으로 보낸다
                var tbl = AttendGiftTableImpl.GiftDict;
                Logger.ZLogInformation("{0}일차 출석 환영합니다~~ 편지 보낼게요!!", attendGiftInfo.HowLongDays);
                Logger.ZLogInformation("출석 보상: {0}", tbl[attendGiftInfo.HowLongDays].ItemName);

                var mailInsertCount = await MysqlManager.Instance.InsertMail(
                    request.ID,
                    tbl[attendGiftInfo.HowLongDays].ItemId,
                    "운영자",
                    Int32.Parse(tbl[attendGiftInfo.HowLongDays].Amount),
                    attendGiftInfo.HowLongDays + "일차 출석 환영해요!",
                    "접속 자주 해주세용");
                
                if (mailInsertCount != 1)
                {
                    Logger.ZLogError("Mail Send Fail!");
                    response.Result = ErrorCode.Mail_Fail_CannotSend;
                    return response;
                }
                
                //선물 지급 날짜 업데이트
                var giftUpdateCount = await MysqlManager.Instance.UpdateAttend(request.ID, true);
                if (giftUpdateCount != 1)
                {
                    Logger.ZLogError("GiftDate Update Fail!");
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
