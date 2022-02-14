using Dapper;
using MySqlConnector;
using System.Security.Cryptography;
using CloudStructures;
using System;
using System.Linq;
using System.Text;

namespace com2us_start;
public class DBManager 
{
    private string GameDBConnectString;
    private string RedisAddress;
    private const string allowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";

    public RedisConnection RedisConn { get; set; }

    //Singleton
    private DBManager() { }
    private static readonly Lazy<DBManager> _instance = new Lazy<DBManager>(() => new DBManager());
    public static DBManager Instance { get { return _instance.Value; } }

    public void Init(IConfiguration conf)
    {
        GameDBConnectString = conf.GetSection("DBConnection")["Mysql"];
        RedisAddress = conf.GetSection("DBConnection")["Redis"];

        var config = new RedisConfig("com2us", "127.0.0.1");
        RedisConn = new RedisConnection(config);
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

    
    
    public string MakeHashingPassword(string saltValue, string pwd)
    {
        var sha = new SHA256Managed();
        byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(saltValue + pwd));
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in hash)
        {
            //16바이트 포맷으로 2글자씩
            stringBuilder.AppendFormat("{0:x2}", b);
        }

        return stringBuilder.ToString();
    }
        
    public string SaltString()
    {
        var bytes = new byte[64];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
        }
        return new string(bytes.Select(x => allowableCharacters[x % allowableCharacters.Length]).ToArray());
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
}
