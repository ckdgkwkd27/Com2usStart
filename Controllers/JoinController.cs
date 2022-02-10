using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace com2us_start.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JoinController : ControllerBase
    {
        [HttpPost]
        public async Task<JoinResponse> Post(JoinRequest request)
        {
            var response = new JoinResponse() { Result = ErrorCode.NONE };
            using (var conn = await com2us_start.DBManager.Instance.GetDBConnection())
            {
                try
                {
                    var count = await conn.ExecuteAsync(@"INSERT com2us.account(ID, Password) Values(@id, @pwd)",
                        new
                        {
                            id = request._ID,
                            pwd = request._Password
                        });

                    if (count != 1)
                    {
                        response.Result = ErrorCode.JOIN_FAIL_DUPLICATE;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    response.Result = ErrorCode.JOIN_FAIL_EXCEPTION;
                    return response;
                }
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

}