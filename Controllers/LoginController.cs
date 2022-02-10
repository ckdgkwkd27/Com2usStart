using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace com2us_start.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger Logger;

        public LoginController(ILogger<LoginController> logger)
        {
            Logger = logger;
        }

        [HttpPost]
        public async Task<LoginResponse> Post(LoginRequest request)
        {
            Logger.LogInformation($"[Request Login] ID:{request._ID}, PW:{request._Password}");

            var response = new LoginResponse();
            response.Result = ErrorCode.NONE;

            using (var conn = await DBManager.Instance.GetDBConnection())
            {
                var memberInfo = await conn.QuerySingleOrDefaultAsync<Member>(
                    @"select ID, Password from com2us.account where ID=@id and Password=@pwd", 
                    new { 
                        id = request._ID, 
                        pwd = request._Password 
                    });

                if (memberInfo == null || string.IsNullOrEmpty(memberInfo.password))
                {
                    response.Result = ErrorCode.LOGIN_FAIL_NOTUSER;
                    return response;
                }
            }

            return response;
        }
    }
}

public class LoginRequest
{
    public string _ID { get; set; }
    public string _Password { get; set; }
}

public class LoginResponse
{
    public ErrorCode Result { get; set; }
}

