﻿using MySqlConnector;

namespace com2us_start;

public class RealDbConnector : IRealDbConnector
{
    private MySqlConnection connection;
    public async Task<MySqlConnection> Connect(string connectString)
    {
        connection = new MySqlConnection(connectString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task Disconnect()
    {
        await connection.CloseAsync();
    }
}
