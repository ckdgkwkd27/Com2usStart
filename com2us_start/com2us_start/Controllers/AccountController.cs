using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace com2us_start.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        DBManager dbManager;
        public AccountController()
        {
            dbManager = new DBManager();
            dbManager.connectionFactory("root", "4321", "com2us");
        }

        [HttpGet]
        public string hello()
        {
            return "HELLO WORLD";
        }

        [HttpGet("all")]
        public List<Member> GetAllMembers()
        {
            return dbManager.GetAllMember();
        }

        [HttpGet("login/{id}/{pwd}")]
        public string Login(string id, string pwd)
        {
            bool ret = dbManager.MemberCheck( $"{id}", $"{pwd}");
            string s = ret ? "Valid Member" : "ERROR: Invalid Member";
            return s;
        }

        [HttpGet("join/{id}/{pwd}")]
        public void Join(string id, string pwd)
        {
            dbManager.InsertSingleMember($"{id}", $"{pwd}");
            Console.WriteLine("Login Success!");
        }
    }
}
