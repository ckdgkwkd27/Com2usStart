using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class ReceiveMailController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConfiguration _conf;

    public ReceiveMailController(ILogger<ReceiveMailController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<ReceiveResponse> Post(ReceiveRequest request)
    {
        var response = new ReceiveResponse() { Result = ErrorCode.None };
        
        try
        {
            var mailList = await MysqlManager.SelectMultipleMailQuery(request.UUID);
            if (mailList.Count == 0)
            {
                _logger.ZLogError("Mail Is Empty!");
                response.Result = ErrorCode.Mail_Fail_Empty;
                return response;
            }

            foreach (var mail in mailList)
            {
                var insertCount = await MysqlManager.InsertItemToInventory(mail.UUID, mail.ItemID, mail.Amount);
                if (insertCount != 1)
                {
                    _logger.ZLogError("Wrong Player ID");
                    response.Result = ErrorCode.Recv_Fail_NotUser;
                    return response;
                }
            }

            var delCount = await MysqlManager.DeleteMails(request.UUID);
            if (delCount == 0)
            {
                _logger.ZLogError("Invalid Item Receive");
                response.Result = ErrorCode.Recv_Fail_InvalidRecv;
                return response;
            }
            
            return response;
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex.ToString());
            response.Result = ErrorCode.Recv_Fail_Exception;
            return response;
        }
    }
}

public class ReceiveRequest
{
    public string ID { get; set; }
    public string UUID { get; set; }
    public string AuthToken { get; set; }
}

public class ReceiveResponse
{
    public ErrorCode Result { get; set; }
}
