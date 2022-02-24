﻿using com2us_start.TableImpl;
using Dapper;
using MySqlConnector;

namespace com2us_start;

public class MysqlManager : IDisposable
{
    private static string? _sGameDBConnectString;
    private static RealDbConnector? _sRealDbConnector;
    private static IConfiguration? _sConf;

    public MysqlManager(IConfiguration sConf, RealDbConnector sRealDbConnector)
    {
        _sRealDbConnector = sRealDbConnector;
        _sConf = sConf;
        _sGameDBConnectString = _sConf.GetSection("DBConnection")["Mysql"];
    }

    public MysqlManager()
    {
        _sGameDBConnectString = _sConf.GetSection("DBConnection")["Mysql"];
        _sRealDbConnector.Connect(_sGameDBConnectString);
    }
    
    //Dispose Pattern
    ~MysqlManager()
    {
        Dispose();    
    }
    
    public static async Task<MySqlConnection> GetDBConnection()
    {
        return await _sRealDbConnector.Connect(_sGameDBConnectString);
    }
    
    public async void Dispose()
    {
        Console.WriteLine("Disposing........................................................................");
        await _sRealDbConnector.Disconnect();
        
        _sGameDBConnectString = null;
        _sRealDbConnector = null;
        _sConf = null;
    }

    //Query를 내부에서 처리 

    //Member
    public static async Task<Int32> InsertAccountQuery(string id, string password, string salt)
    {
        await using var conn = await GetDBConnection();
        var count = await conn.ExecuteAsync(
            @"INSERT com2us.account(ID, Password, Salt) Values(@ID, @Pwd, @Salt)",
            new
            {
                ID = id,
                Pwd = password,
                Salt = salt,
            });

        return count;
    }

    public static async Task<Member?> SelectMemberQuery(string Id)
    {
        await using var conn = await GetDBConnection();
        var memberInfo = await conn.QuerySingleOrDefaultAsync<Member>(
            @"select * from com2us.account where ID=@id",
            new { id = Id });
        
        return memberInfo;
    }

    //GamePlayer
    public static async Task<Int32> InsertPlayer(string uuid, string id, Int32 level, Int32 exp, Int32 gameMoney)
    {
        await using var conn = await GetDBConnection();
        var count = await conn.ExecuteAsync(
            @"INSERT com2us.gameplayer(UUID, ID, Level, Exp, GameMoney, AttendDate, GiftDate, HowLongDays) VALUES(@UUID, @ID, @Level, @Exp, @GameMoney, @NowDate, @UnixDate, 0)",
            new
            {
                UUID = uuid,
                ID = id,
                Level = level,
                Exp = exp,
                GameMoney = gameMoney,
                NowDate = DateTime.Now,
                UnixDate = DateTime.UnixEpoch,
            });
        
        return count;
    }

    public static async Task<GamePlayer?> SelectGamePlayerQuery(string Id)
    {
        await using var conn = await GetDBConnection();
        var playerInfo = await conn.QuerySingleOrDefaultAsync<GamePlayer>(
            @"select * from com2us.gameplayer where UUID=@id",
            new { id = Id });

        return playerInfo;
    }
    
    public static async Task<Int32> UpdatePlayerAttend(string Id, bool isGiftGiven = false)
    {
        await using var conn = await GetDBConnection();
        Int32 count;

        if (isGiftGiven)
        {
            count = await conn.ExecuteAsync(
                @"UPDATE com2us.gameplayer SET AttendDate=@NowDate, GiftDate=@NowDate, HowLongDays = HowLongDays+1 WHERE UUID=@id",
                new { @NowDate = DateTime.Now, id = Id });
        }
        else
        {
            count = await conn.ExecuteAsync(
                @"UPDATE com2us.gameplayer SET AttendDate=@NowDate WHERE UUID=@id",
                new { @NowDate = DateTime.Now, id = Id });
        }

        return count;
    }

    //PlayerRobotmon
    public static async Task<PlayerRobotmon?> SelectPlayerRobotmonQuery(string Id)
    {
        await using var conn = await GetDBConnection();
        var playerRobotmonInfo = await conn.QuerySingleOrDefaultAsync<PlayerRobotmon>(
            @"select * from com2us.playerrobotmon where UUID=@id",
            new { id = Id });

        return playerRobotmonInfo;
    }
    

    //Mail
    public static async Task<Int32> InsertMail(string uuid, string recvId, string? itemId, string sendName, Int32 amount, string title,
        string content)
    {
        await using var conn = await GetDBConnection();
        var count = await conn.ExecuteAsync(
            @"INSERT com2us.mail(UUID, MailID, RecvID, ItemID, SendName, RecvDate, Amount, Title, Content) VALUES(@_uuid, @_mailId, @_recvId, @_itemId,@_sendName, @_recvDate, @_amount, @_title, @_content)",
            new
            {
                @_uuid = uuid,
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
    
    public static async Task<Int32> InsertAttendOperationMail(string uuid, string? Id, GamePlayer player)
    {
        var tbl = AttendGiftTableImpl.GiftDict;
        var count = await InsertMail(
            uuid,
            Id,
            tbl[player.HowLongDays].ItemId,
            "운영자",
            Int32.Parse(tbl[player.HowLongDays].Amount),
            player.HowLongDays + "일차 출석 환영해요!",
            "접속 자주 해주세용");
        
        return count;
    }

    public static async Task<List<Mail>> SelectMultipleMailQuery(string recvId)
    {
        await using var conn = await GetDBConnection();
        var multi = await conn.QueryMultipleAsync(
            @"SELECT * FROM com2us.mail WHERE UUID=@id",
            new { id = recvId }).ConfigureAwait(false);

        var mails = multi.Read<Mail>().ToList();
        
        return mails;
    }

    public static async Task<Int32> DeleteMails(string recvId)
    {
        await using var conn = await GetDBConnection();
        var count = await conn.ExecuteAsync(
            @"DELETE FROM com2us.mail WHERE UUID=@ID",
            new { ID = recvId });
        
        return count;
    }
    
    //Inventory
    public static async Task<Int32> InsertItemToInventory(string uuid, string? itemId, Int32 amount)
    {
        await using var conn = await GetDBConnection();
        var count = await conn.ExecuteAsync(
            @"INSERT com2us.inventory(UUID, ItemID, Amount) VALUES(@_uuid, @_itemId, @_amount)",
            new { _uuid = uuid, _itemId = itemId, _amount = amount, });
        
        return count;
    }

    public static async Task<List<Inventory>> SelectMultipleInventoryQuery(string uuid)
    {
        await using var conn = await GetDBConnection();
        var multi = await conn.QueryMultipleAsync(
            @"SELECT * FROM com2us.inventory WHERE UUID=@_uuid",
            new { _uuid = uuid }).ConfigureAwait(false);

        var items = multi.Read<Inventory>().ToList();
        
        return items;
    }
    
    //Item
    public static async Task<Item> SelectItemQuery(string itemId)
    {
        await using var conn = await GetDBConnection();
        var item = await conn.QuerySingleOrDefaultAsync<Item>(
            @"SELECT * FROM com2us.item WHERE ItemID=@ItemID",
            new { ItemID = itemId });
        
        return item;
    }

    public static async Task<List<InventoryItem>> SelectAllInventoryItems(string uuid)
    {
        await using var conn = await GetDBConnection();
        var multi = await conn.QueryMultipleAsync(
            @"select item.ItemId, item.ItemName, item.ItemType, inven.amount
                from Item item left join Inventory inven
                on item.ItemId = inven.ItemId
                where inven.UUID=@_uuid
                union
                select item.ItemId, item.ItemName, item.ItemType, inven.amount
                from Item item right join Inventory inven
                on item.ItemId = inven.ItemId
                where inven.UUID=@_uuid",
            new { _uuid = uuid, }).ConfigureAwait(false);
        var items = multi.Read<InventoryItem>().ToList();
        
        return items;
    }
}
