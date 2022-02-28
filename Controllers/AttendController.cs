using CloudStructures.Structures;
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

    //컨트롤러에 MysqlManager 정보 주입 
    public AttendController(ILogger<AttendController> logger, IConfiguration conf, IRealDbConnector realDbConnector)
    {
        _logger = logger;
        _conf = conf;
        _realDbConnector = realDbConnector;
    }

    [HttpPost]
    public async Task<AttendResponse> Post(AttendRequest request)
    {
        var response = new AttendResponse() { Result = ErrorCode.None };

        try
        {
            using MysqlManager manager = new MysqlManager(_conf, _realDbConnector);
            
            var attendInfo = await manager.SelectGamePlayerQuery(request.UUID);
            if (attendInfo == null)
            {
                _logger.ZLogError("Wrong User ID");
                response.Result = ErrorCode.Attend_Fail_NotUser;
                return response;
            }
            
            var elapsed = DateTime.Now - attendInfo.AttendDate;
            if(elapsed.Days >= 1)
            {
                var memberUpdateCount = await manager.UpdatePlayerAttend(request.UUID);
                if (memberUpdateCount != 1)
                {
                    _logger.ZLogError("ERROR: Attend Update Failed!");
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

public class AttendRequest
{
    public string ID { get; set; }
    public string UUID { get; set; }
    public string AuthToken { get; set; }
}

public class AttendResponse
{
    public ErrorCode Result { get; set; }
}
