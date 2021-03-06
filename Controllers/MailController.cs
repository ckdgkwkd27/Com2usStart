using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class MailController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConfiguration _conf;
    private readonly IRealDbConnector _realDbConnector;

    public MailController(ILogger<MailController> logger, IConfiguration conf, IRealDbConnector realDbConnector)
    {
        _logger = logger;
        _conf = conf;
        _realDbConnector = realDbConnector;
    }

    [HttpPost]
    public async Task<MailResponse> Post(MailRequest request)
    {
        var response = new MailResponse();

        try
        {
            var mailList = await _realDbConnector.SelectMail(request.PlayerID);
            if (mailList.Count == 0)
            {
                _logger.ZLogError("Mail Is Empty!");
                response.RecvMail = null;
                response.Result = ErrorCode.Mail_Fail_Empty;
                return response;
            }
            
            response.RecvMail = mailList;
            return response;
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex.ToString());
            response.RecvMail = new List<Mail>();
            response.Result = ErrorCode.Mail_Fail_Exception;
            return response;
        }
    }
}

public class MailRequest
{
    public string ID { get; set; }
    public string PlayerID { get; set; }
    public string AuthToken { get; set; }
}

public class MailResponse
{
    public MailResponse() { Result = ErrorCode.None;}
    public ErrorCode Result { get; set; }
    public List<Mail> RecvMail { get; set; }
}