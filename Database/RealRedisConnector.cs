using System.Security.Cryptography;
using CloudStructures;
using CloudStructures.Structures;

namespace com2us_start;

public class RealRedisConnector : IRealRedisConnector
{
    private string RedisAddress;
    private const string allowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
    public RedisConnection RedisConn { get; set; }

    ~RealRedisConnector()
    {
        Dispose();    
    }
    
    public void Dispose()
    {
        RedisConn.GetConnection().Close();
        RedisConn.GetConnection().Dispose();
    }
    
    public RedisConnection GetConnector()
    {
        return RedisConn;
    }
    
    public void InitConnector(IConfiguration conf)
    {
        RedisAddress = conf.GetSection("DBConnection")["Redis"];
        var config = new RedisConfig("com2us", "127.0.0.1");
        RedisConn = new RedisConnection(config);
    }
    public string AuthToken()
    {
        var bytes = new byte[25];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
        }
        return new string(bytes.Select(x => allowableCharacters[x % allowableCharacters.Length]).ToArray());
    }

    public async Task<ErrorCode> TokenCheck(string? id, string? authToken)
    {
        //Redis에서 인증 토큰 체크
        var defaultExpiry = TimeSpan.FromDays(1);
        var redisId = new RedisString<string>(RedisConn, id, defaultExpiry);
        var stringResult = await redisId.GetAsync();
        if (stringResult.Value != authToken)
        {
            return ErrorCode.Token_Fail_NotAuthorized;
        }
        return ErrorCode.None;
    }
}
