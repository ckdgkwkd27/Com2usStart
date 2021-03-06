using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class ReceiveMailController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConfiguration _conf;
    private readonly IRealDbConnector _realDbConnector;

    public ReceiveMailController(ILogger<ReceiveMailController> logger, IConfiguration conf, IRealDbConnector realDbConnector)
    {
        _logger = logger;
        _conf = conf;
        _realDbConnector = realDbConnector;
    }

    [HttpPost]
    public async Task<ReceiveResponse> Post(ReceiveRequest request)
    {
        var response = new ReceiveResponse() { Result = ErrorCode.None };
        
        try
        {
            var mailList = await _realDbConnector.SelectMail(request.PlayerID);
            if (mailList.Count == 0)
            {
                _logger.ZLogError("Mail Is Empty!");
                response.Result = ErrorCode.Mail_Fail_Empty;
                return response;
            }

            foreach (var mail in mailList)
            {
                var insertCount = await _realDbConnector.InsertItemToInventory(mail.PlayerID, mail.ItemID, mail.Amount, mail.ItemName, mail.ItemType);
                if (insertCount != 1)
                {
                    _logger.ZLogError("Wrong Player ID");
                    response.Result = ErrorCode.Recv_Fail_NotUser;
                    return response;
                }
            }

            var delCount = await _realDbConnector.DeleteMails(request.PlayerID);
            if (delCount == 0)
            {
                _logger.ZLogError("Receiving Item is Failed");
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
    public string PlayerID { get; set; }
    public string AuthToken { get; set; }
}

public class ReceiveResponse
{
    public ReceiveResponse() { Result = ErrorCode.None;}
    public ErrorCode Result { get; set; }
}
