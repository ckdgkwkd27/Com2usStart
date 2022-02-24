using MySqlConnector;

namespace com2us_start;

public interface IRealDbConnector
{
    Task<MySqlConnection> Connect(string connectString);
    Task Disconnect();
}
