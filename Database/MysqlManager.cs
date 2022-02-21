using System.Security.Cryptography;
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
    public async Task<Int32> InsertAccountQuery(string id, string password, string salt)
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

    public async Task<Member?> SelectMemberQuery(string Id)
    {
        await using var conn = await GetDBConnection();
        var memberInfo = await conn.QuerySingleOrDefaultAsync<Member>(
            @"select ID, Password, Salt from com2us.account where ID=@id", 
            new { id = Id});
            
        return memberInfo;
    }
    
    //GamePlayer
    public async Task<Int32> InsertPlayer(string id, Int32 level, Int32 exp, Int32 gameMoney)
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
    
    public async Task<GamePlayer?> SelectGamePlayerQuery(string Id)
    {
        await using var conn = await GetDBConnection();
        var playerInfo = await conn.QuerySingleOrDefaultAsync<GamePlayer>(
            @"select ID, Level, Exp, GameMoney from com2us.gameplayer where ID=@id", 
            new { id = Id});
            
        return playerInfo;
    }
    
    //PlayerRobotmon
    public async Task<PlayerRobotmon?> SelectPlayerRobotmonQuery(string Id)
    {
        await using var conn = await GetDBConnection();
        var playerRobotmonInfo = await conn.QuerySingleOrDefaultAsync<PlayerRobotmon>(
            @"select * from com2us.playerrobotmon where PlayerID=@id", 
            new { id = Id});
            
        return playerRobotmonInfo;
    }
    
    //Attendance
    public async Task<Int32> InsertAttend(string id)
    {
        await using var conn = await GetDBConnection();
        var count = await conn.ExecuteAsync(
            @"INSERT com2us.attendance(ID, AttendDate, GiftDate, HowLongDays) VALUES(@Id, @NowDate, @OldDate, 0)",
            new
            {
                @Id = id,
                @NowDate = DateTime.Now,
                @OldDate = DateTime.UnixEpoch,
            });
        
        return count;
    }

    public async Task<Attendance?> SelectAttendQuery(string Id)
    {
        await using var conn = await GetDBConnection();
        var attendInfo = await conn.QuerySingleOrDefaultAsync<Attendance>(
            @"SELECT ID, AttendDate, GiftDate, HowLongDays FROM com2us.attendance WHERE ID=@id",
            new { id = Id });
        return attendInfo;
    }

    public async Task<Int32> UpdateAttend(string Id, bool isGiftGiven = false)
    {
        await using var conn = await GetDBConnection();
        Int32 count;
        
        if (isGiftGiven)
        {
            count = await conn.ExecuteAsync(
                @"UPDATE com2us.attendance SET AttendDate=@Now, GiftDate=@Now, HowLongDays = HowLongDays+1 WHERE ID=@id",
                new {@Now = DateTime.Now, id = Id});
        }
        else
        {
            count = await conn.ExecuteAsync(
                @"UPDATE com2us.attendance SET AttendDate=@Now WHERE ID=@id",
                new {@Now = DateTime.Now, id = Id});
        }
       
        return count;
    }
    
    
    //Mail
    public async Task<Int32> InsertMail(string recvId, string? itemId, string sendName, Int32 amount, string title, string content)
    {
        await using var conn = await GetDBConnection();
        var count = await conn.ExecuteAsync(
            @"INSERT com2us.mail(MailID, RecvID, ItemID, SendName, RecvDate, Amount, Title, Content) VALUES(@_mailId, @_recvId, @_itemId,@_sendName, @_recvDate, @_amount, @_title, @_content)",
            new 
            {
                @_mailId = Guid.NewGuid().ToString(),
                @_recvId = recvId,
                @_itemId = itemId,
                @_sendName = sendName,
                @_recvDate = DateTime.Now,
                @_amount = amount,
                @_title = title,
                @_content = content
            });
        
        return count;
    }
    
    public async Task<List<Mail>> SelectMultipleMailQuery(string recvId)
    {
        await using var conn = await GetDBConnection();
        var multi = await conn.QueryMultipleAsync(
            @"SELECT * FROM com2us.mail WHERE RecvID=@id",
            new { id = recvId }).ConfigureAwait(false);

        var mails = multi.Read<Mail>().ToList();
        return mails;
    }
}
