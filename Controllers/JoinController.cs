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
        Logger.ZLogDebug($"[Request Join] ID:{request.ID}, PW:{request.Password}");
        
        var response = new JoinResponse() { Result = ErrorCode.None };

        var saltValue = CryptoManager.Instance.SaltString();
        var hashingPassword = CryptoManager.Instance.MakeHashingPassword(saltValue, request.Password);
        
        try
        {
            var accountInsertCount =  await MysqlManager.Instance.InsertAccountQuery(request.ID, hashingPassword, saltValue);
            if (accountInsertCount != 1)
            {
                Logger.ZLogError("ERROR: Account Duplicate");
                response.Result = ErrorCode.Join_Fail_Duplicate;
                return response;
            }


            if (response.Result == ErrorCode.None)
            {
                Logger.ZLogInformation("Join Success!! Welcome {0}", request.ID);
            }
            
            //Player Data 생성
            var playerInsertCount = await MysqlManager.Instance.InsertPlayer(
                id: request.ID,
                level: 1,
                exp: 0,
                gameMoney: 0
            );

            if (playerInsertCount != 1)
            {
                Logger.ZLogError("ERROR: Player Create Failed!");
                response.Result = ErrorCode.Join_Fail_PlayerFailed;
                return response;
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
