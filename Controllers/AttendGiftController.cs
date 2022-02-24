using com2us_start.TableImpl;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class AttendGiftController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConfiguration _conf;
    private readonly IRealDbConnector _realDbConnector;
    private readonly Int32 _testMinutesLimit;

    public AttendGiftController(ILogger<AttendGiftController> logger, IConfiguration conf, IRealDbConnector realDbConnector)
    {
        _logger = logger;
        _conf = conf;
        _realDbConnector = realDbConnector;
        _testMinutesLimit = 1;
    }

    [HttpPost]
    public async Task<AttendResponse> Post(AttendRequest request)
    {
        var response = new AttendResponse() { Result = ErrorCode.None };

        try
        {
            //Attendance 테이블 조회해서 GiftDate가 null이거나 Time Limit을 넘었으면 선물 지급
            using MysqlManager manager = new MysqlManager(_conf, _realDbConnector);
            await manager.GetDbConnection();
            
            var attendGiftPlayerInfo = await manager.SelectGamePlayerQuery(request.UUID);
            if (attendGiftPlayerInfo == null)
            {
                _logger.ZLogError("Wrong User ID");
                response.Result = ErrorCode.Attend_Fail_NotUser;
                return response;
            }
            
            TimeSpan elapsed = DateTime.Now - attendGiftPlayerInfo.GiftDate;

            //현재 테스트를 위해 1분단위로 출석 가능
            if (elapsed.Minutes >= _testMinutesLimit)
            {
                //출석보상 편지를 우편함으로 보낸다
                var tbl = AttendGiftTableImpl.GiftDict;
                _logger.ZLogInformation("{0}일차 출석 환영합니다~~ 편지 보낼게요!!", attendGiftPlayerInfo.HowLongDays);
                _logger.ZLogInformation("출석 보상: {0}", tbl[attendGiftPlayerInfo.HowLongDays].ItemName);

                var mailInsertCount = await manager.InsertAttendOperationMail(request.UUID, null, attendGiftPlayerInfo);
                
                if (mailInsertCount != 1)
                {
                    _logger.ZLogError("Mail Send Fail!");
                    response.Result = ErrorCode.Mail_Fail_CannotSend;
                    return response;
                }
                
                //선물 지급 날짜 업데이트
                var giftUpdateCount = await manager.UpdatePlayerAttend(request.UUID, true);
                if (giftUpdateCount != 1)
                {
                    _logger.ZLogError("GiftDate Update Fail!");
                    response.Result = ErrorCode.Attend_Fail_NotUser;
                    return response;
                }
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex.ToString());
            response.Result = ErrorCode.Attend_Fail_Exception;
            return response;
        }
    }
}
