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

    public AttendController(ILogger<AttendController> logger)
    {
        _logger = logger;
        
    }

    [HttpPost]
    public async Task<AttendResponse> Post(AttendRequest request)
    {
        var response = new AttendResponse() { Result = ErrorCode.None };

        try
        {
            var attendInfo = await MysqlManager.SelectGamePlayerQuery(request.UUID);
            if (attendInfo == null)
            {
                _logger.ZLogError("Wrong User ID");
                response.Result = ErrorCode.Attend_Fail_NotUser;
                return response;
            }
            
            var elapsed = DateTime.Now - attendInfo.AttendDate;
            if(elapsed.Days >= 1)
            {
                var memberUpdateCount = await MysqlManager.UpdatePlayerAttend(request.UUID);
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
