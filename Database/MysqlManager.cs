﻿using System.Security.Cryptography;
using System.Text;
using Dapper;
using MySqlConnector;

namespace com2us_start;

public class MysqlManager
{
    private string GameDBConnectString;
    
    //Singleton
    private MysqlManager() { }
    private static readonly Lazy<MysqlManager> _instance = new Lazy<MysqlManager>(() => new MysqlManager());
    public static MysqlManager Instance { get { return _instance.Value; } }
    
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

    //Query를 내부에서 처리 
    
    //Member
    public async Task<int> InsertAccountQuery(string id, string password, string salt)
    {
        await using var conn = await GetDBConnection();
        var count = await conn.ExecuteAsync(@"INSERT com2us.account(ID, Password, Salt) Values(@ID, @Pwd, @Salt)",
            new
            {
                ID = id,
                Pwd = password,
                Salt = salt,
            });
        return count;
    }

    public async Task<Member> SelectMemberQuery(string Id)
    {
        await using var conn = await GetDBConnection();
        var memberInfo = await conn.QuerySingleOrDefaultAsync<Member>(
            @"select ID, Password, Salt from com2us.account where ID=@id", 
            new { id = Id});
            
        return memberInfo;
    }
    
    //GamePlayer
    public async Task<int> InsertPlayer(string id, int level, int exp, int gameMoney)
    {
        await using var conn = await GetDBConnection();
        var count = await conn.ExecuteAsync(
            @"INSERT com2us.gameplayer(ID, Level, Exp, GameMoney) VALUES(@ID, @Level, @Exp, @GameMoney)",
            new
            {
                ID = id,
                Level = level,
                Exp = exp,
                GameMoney = gameMoney,
            });

        return count;
    }
    
    public async Task<GamePlayer> SelectGamePlayerQuery(string Id)
    {
        await using var conn = await GetDBConnection();
        var playerInfo = await conn.QuerySingleOrDefaultAsync<GamePlayer>(
            @"select ID, Level, Exp, GameMoney from com2us.gameplayer where ID=@id", 
            new { id = Id});
            
        return playerInfo;
    }
    
    //PlayerRobotmon
    public async Task<PlayerRobotmon> SelectPlayerRobotmonQuery(string Id)
    {
        await using var conn = await GetDBConnection();
        var playerRobotmonInfo = await conn.QuerySingleOrDefaultAsync<PlayerRobotmon>(
            @"select * from com2us.playerrobotmon where PlayerID=@id", 
            new { id = Id});
            
        return playerRobotmonInfo;
    }
}
