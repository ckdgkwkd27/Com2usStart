using CloudStructures.Structures;
using com2us_start.TableImpl;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class AttendController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConfiguration _conf;
    private readonly IRealDbConnector _realDbConnector;
    private readonly Int32 _testMinutesLimit;
    private GamePlayer? _attendInfo;

    //Dependency Injection
    public AttendController(ILogger<AttendController> logger, IConfiguration conf, IRealDbConnector realDbConnector)
    {
        _logger = logger;
        _conf = conf;
        _realDbConnector = realDbConnector;
        _testMinutesLimit = 1;
    }

    [HttpPost]
    public async Task<AttendResponse> Post(AttendRequest request)
    {
        var response = new AttendResponse();

        try
        {
            _attendInfo = await _realDbConnector.SelectGamePlayer(request.PlayerID);
            if (_attendInfo == null)
            {
                _logger.ZLogError("Wrong User ID");
                response.Result = ErrorCode.Attend_Fail_NotUser;
                return response;
            }
            
            var attendElapsed = DateTime.Now - _attendInfo.AttendDate;
            
            if(attendElapsed.Days >= 1)
            {
                var memberUpdateCount = await _realDbConnector.UpdatePlayerAttend(request.PlayerID);
                if (memberUpdateCount != 1)
                {
                    _logger.ZLogError("ERROR: Attend Update Failed!");
                    response.Result = ErrorCode.Attend_Fail_NotUser;
                    return response;
                }
            }

            return await GiveAttendGift(request);
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex.ToString());
            response.Result = ErrorCode.Attend_Fail_Exception;
            return response;
        }
    }

    public async Task<AttendResponse> GiveAttendGift(AttendRequest request)
    {
        var response = new AttendResponse() { Result = ErrorCode.None };
        var giftElapsed = DateTime.Now - _attendInfo.GiftDate;
        
        if (giftElapsed.Minutes >= _testMinutesLimit)
        {
            var tbl = AttendGiftTableImpl.GiftDict;
            _logger.ZLogDebug("{0}일차 출석 환영합니다~~ 편지 보낼게요!!", _attendInfo.HowLongDays);
            _logger.ZLogDebug("출석 보상: {0}", tbl[_attendInfo.HowLongDays].ItemName);
                
            var mailInsertCount = await _realDbConnector.InsertAttendOperationMail(request.PlayerID, null, _attendInfo);
                
            if (mailInsertCount != 1)
            {
                _logger.ZLogError("Mail Send Fail!");
                response.Result = ErrorCode.Mail_Fail_CannotSend;
                return response;
            }
                
            //선물 지급 날짜 업데이트
            var giftUpdateCount = await _realDbConnector.UpdatePlayerAttend(request.PlayerID, true);
            if (giftUpdateCount != 1)
            {
                _logger.ZLogError("GiftDate Update Fail!");
                response.Result = ErrorCode.Attend_Fail_NotUser;
                return response;
            }
        }

        return response;
    }
}

public class AttendRequest
{
    public string ID { get; set; }
    public string PlayerID { get; set; }
    public string AuthToken { get; set; }
}

public class AttendResponse
{
    public AttendResponse() { Result = ErrorCode.None;}
    public ErrorCode Result { get; set; }
}
