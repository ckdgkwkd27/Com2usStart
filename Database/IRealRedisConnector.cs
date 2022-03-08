using CloudStructures;

namespace com2us_start;

public interface IRealRedisConnector : IDisposable
{
    string AuthToken();
}
