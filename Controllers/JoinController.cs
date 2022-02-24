using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class JoinController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConfiguration _conf;

    public JoinController(ILogger<JoinController> logger)
    {
        _logger = logger;
        //_conf = conf;
    }
    
    [HttpPost]
    public async Task<JoinResponse> Post(JoinRequest request)
    {
        //ZLogger 적용
        _logger.ZLogDebug($"[Request Join] ID:{request.ID}, PW:{request.Password}");
        
        var response = new JoinResponse() { Result = ErrorCode.None };

        var saltValue = CryptoManager.Instance.SaltString();
        var hashingPassword = CryptoManager.Instance.MakeHashingPassword(saltValue, request.Password);
        
        try
        {
            var accountInsertCount =  await MysqlManager.InsertAccountQuery(request.ID, hashingPassword, saltValue);
            if (accountInsertCount != 1)
            {
                _logger.ZLogError("ERROR: Account Duplicate");
                response.Result = ErrorCode.Join_Fail_Duplicate;
                return response;
            }
            
            if (response.Result == ErrorCode.None)
            {
                _logger.ZLogInformation("Join Success!! Welcome {0}", request.ID);
            }
        }
        
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            response.Result = ErrorCode.Join_Fail_Exception;
            return response;
        }
        return response;
    }
}

public class JoinRequest
{
    public string ID { get; set; }
    public string Password { get; set; }
}

public class JoinResponse
{
    public ErrorCode Result { get; set; }
}
