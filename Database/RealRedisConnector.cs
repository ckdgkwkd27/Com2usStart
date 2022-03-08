using System.Security.Cryptography;
using CloudStructures;
using CloudStructures.Structures;
using com2us_start;

namespace com2us_start;

public class RealRedisConnector : IRealRedisConnector
{
    private const string AllowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
    public static RedisConnection RedisConn { get; set; }

    public RealRedisConnector(IConfiguration conf)
    {
        var config = new RedisConfig("com2us", "127.0.0.1");
        RedisConn = new RedisConnection(config);
    }
    
    public void Dispose()
    {
        Console.WriteLine("Disposing........................................................................");
        //RedisConn.GetConnection().Close();
        //RedisConn.GetConnection().Dispose();
    }
    
    public string AuthToken()
    {
        var bytes = new byte[25];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
        }
        return new string(bytes.Select(x => AllowableCharacters[x % AllowableCharacters.Length]).ToArray());
    }

    public static async Task<ErrorCode> TokenCheck(string? id, string? authToken)
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
