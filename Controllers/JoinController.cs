using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class JoinController : ControllerBase
{
    private readonly ILogger Logger;

    public JoinController(ILogger<JoinController> logger)
    {
        Logger = logger;
    }
    
    [HttpPost]
    public async Task<JoinResponse> Post(JoinRequest request)
    {
        //ZLogger 적용
        Logger.ZLogDebug($"[Request Join] ID:{request._ID}, PW:{request._Password}");
        
        var response = new JoinResponse() { Result = ErrorCode.None };

        var saltValue = DBManager.Instance.SaltString();
        var hashingPassword = DBManager.Instance.MakeHashingPassword(saltValue, request._Password);
            
        using (var conn = await DBManager.Instance.GetDBConnection())
        {
            try
            {
                var count = await conn.ExecuteAsync(@"INSERT com2us.account(ID, Password, Salt) Values(@id, @pwd, @salt)",
                    new
                    {
                        id = request._ID,
                        pwd = hashingPassword,
                        salt = saltValue,
                    });

                if (count != 1)
                {
                    response.Result = ErrorCode.Join_Fail_Duplicate;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Result = ErrorCode.Join_Fail_Exception;
                return response;
            }
        }

        if (response.Result == ErrorCode.None)
        {
            Logger.ZLogInformation("Join Success!! Welcome {0}", request._ID);
        }
        return response;
    }
}

public class JoinRequest
{
    public string _ID { get; set; }
    public string _Password { get; set; }
}

public class JoinResponse
{
    public ErrorCode Result { get; set; }
}
