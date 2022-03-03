using System.Security.Cryptography;
using CloudStructures;
using CloudStructures.Structures;

namespace com2us_start;

public class RedisManager : IDisposable
{
    public static IRealRedisConnector _realRedisConnector;
    
    public RedisManager(IConfiguration conf, IRealRedisConnector realRedisConnector)
    {
        _realRedisConnector = realRedisConnector; 
        _realRedisConnector.InitConnector(conf);
    }

    ~RedisManager()
    {
        Dispose();
    }
    
    public RedisConnection RedisConn { get; set; }

    public string AuthToken()
    {
        return _realRedisConnector.AuthToken();
    }

    public async Task<ErrorCode> TokenCheck(string? id, string? authToken)
    {
        return await _realRedisConnector.TokenCheck(id, authToken);
    }

    public void Dispose()
    {
        _realRedisConnector.Dispose();
    }
}
