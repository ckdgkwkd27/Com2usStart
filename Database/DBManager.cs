using Dapper;
using MySqlConnector;
using System;
using System.Linq;

namespace com2us_start
{
    public class DBManager 
    {
        private string GameDBConnectString;

        //Singleton
        private DBManager() { }
        private static readonly Lazy<DBManager> _instance = new Lazy<DBManager>(() => new DBManager());
        public static DBManager Instance { get { return _instance.Value; } }

        public void Init(IConfiguration conf)
        {
            GameDBConnectString = conf.GetSection("DBConnection")["Mysql"];
        }

        public async Task<MySqlConnection> GetDBConnection()
        {
            return await GetOpenMysqlConnection(GameDBConnectString);
        }

        public async Task<MySqlConnection> GetOpenMysqlConnection(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
