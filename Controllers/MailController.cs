using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class MailController : ControllerBase
{
    private readonly ILogger Logger;

    public MailController(ILogger<MailController> logger)
    {
        Logger = logger;
    }

    [HttpPost]
    public async Task<MailResponse> Post(MailRequest request)
    {
        Logger.ZLogDebug($"[Request Join] ID:{request.ID}, Token:{request.AuthToken}");

        var response = new MailResponse() { Result = ErrorCode.None };

        try
        {
            response.Result = await RedisManager.Instance.TokenCheck(request.ID, request.AuthToken);
            if (response.Result == ErrorCode.Token_Fail_NotAuthorized)
            {
                return response;
            }

            var mailList = await MysqlManager.Instance.SelectMultipleMailQuery(request.ID);
            if (mailList.Count == 0)
            {
                Logger.ZLogError("Mail Is Empty!");
                response.RecvMail = null;
                response.Result = ErrorCode.Mail_Fail_Empty;
                return response;
            }
            
            response.RecvMail = mailList;
            return response;
        }
        catch (Exception ex)
        {
            Logger.ZLogError(ex.ToString());
            response.RecvMail = null;
            response.Result = ErrorCode.Mail_Fail_Exception;
            return response;
        }
    }
}

public class MailRequest
{
    public string ID { get; set; }
    public string AuthToken { get; set; }
}

public class MailResponse 
{
    public ErrorCode Result { get; set; }
    public List<Mail> RecvMail { get; set; }
}