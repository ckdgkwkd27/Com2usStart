using CloudStructures;

namespace com2us_start;

public interface IRealRedisConnector : IDisposable
{
    RedisConnection GetConnector();
    void InitConnector(IConfiguration conf);
    string AuthToken();
    Task<ErrorCode> TokenCheck(string? id, string? authToken);
}
