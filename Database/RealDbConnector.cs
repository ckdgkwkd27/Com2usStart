using System.Data;
using com2us_start.TableImpl;
using Dapper;
using MySqlConnector;

namespace com2us_start;

public class RealDbConnector : IRealDbConnector
{
    private readonly MySqlConnection _connection;

    public RealDbConnector(IConfiguration conf)
    {
        var connectString = conf.GetSection("DBConnection")["Mysql"];    
        _connection = new MySqlConnection(connectString);

        Connect();
    }

    public void Connect()
    {
        _connection.Open();
    }

    public async Task Disconnect()
    {
        await _connection.CloseAsync();
    }
    
    public void Dispose()
    {
        Console.WriteLine("Disposing........................................................................");
        _connection.Close();
        _connection.Dispose();
    }
    
    public async Task<int> InsertAccountQuery(string id, string password, string salt)
    {
        var member = await SelectMember(id);
        if (member != null)
        {
            return -1;
        }
        
        var count = await _connection.ExecuteAsync(
            @"INSERT com2us.account(ID, Password, Salt) Values(@ID, @Pwd, @Salt)",
            new
            {
                ID = id,
                Pwd = password,
                Salt = salt,
            });

        return count;
    }

    public async Task<Member?> SelectMember(string Id)
    {
        var memberInfo = await _connection.QuerySingleOrDefaultAsync<Member>(
            @"select * from com2us.account where ID=@id",
            new { id = Id });

        return memberInfo;
    }

    public async Task<int> InsertPlayer(string id, int level, int exp, int gameMoney)
    {
        var count = await _connection.ExecuteAsync(
            @"INSERT com2us.gameplayer(ID, Level, Exp, GameMoney, AttendDate, GiftDate, HowLongDays) VALUES(@ID, @Level, @Exp, @GameMoney, @NowDate, @UnixDate, 0)",
            new
            {
                ID = id,
                Level = level,
                Exp = exp,
                GameMoney = gameMoney,
                NowDate = DateTime.Now,
                UnixDate = DateTime.UnixEpoch,
            });

        return count;
    }

    public async Task<GamePlayer?> SelectGamePlayer(string Id)
    {
        var playerInfo = await _connection.QuerySingleOrDefaultAsync<GamePlayer>(
            @"select * from com2us.gameplayer where PlayerID=@id",
            new { id = Id });

        return playerInfo;
    }

    public async Task<int> UpdatePlayerAttend(string Id, bool isGiftGiven = false)
    {
        Int32 count;
        if (isGiftGiven)
        {
            count = await _connection.ExecuteAsync(
                @"UPDATE com2us.gameplayer SET AttendDate=@NowDate, GiftDate=@NowDate, HowLongDays = HowLongDays+1 WHERE PlayerID=@id",
                new { @NowDate = DateTime.Now, id = Id });
        }
        else
        {
            count = await _connection.ExecuteAsync(
                @"UPDATE com2us.gameplayer SET AttendDate=@NowDate WHERE PlayerID=@id",
                new { @NowDate = DateTime.Now, id = Id });
        }

        return count;
    }

    public async Task<int> InsertMail(string playerId, string recvId, string? itemId, string sendName, int amount, 
        string title, string content, string itemName, string itemType, Int32 money = 0)
    {
        var count = await _connection.ExecuteAsync(
            @"INSERT com2us.mail(PlayerID, MailID, RecvID, ItemID, SendName, SendDate, Amount, Title, Content, Money, ItemName, ItemType, IsDeleted) 
                VALUES(@_playerId, @_mailId, @_recvId, @_itemId,@_sendName, @_sendDate, @_amount, @_title, @_content, @_money, @_itemName, @_itemType, 0)",
            new
            { 
                _playerId = playerId,
                _mailId = Guid.NewGuid().ToString(),
                _recvId = recvId,
                _itemId = itemId,
                _itemName = itemName,
                _itemType = itemType,
                _sendName = sendName,
                _sendDate = DateTime.Now,
                _amount = amount,
                _title = title,
                _content = content,
                _money = money,
            });

        return count;
    }

    public async Task<int> InsertAttendOperationMail(string playerId, string? Id, GamePlayer player)
    {
        var tbl = AttendGiftTableImpl.GiftDict;
        var item = await SelectItemQuery(tbl[player.HowLongDays].ItemId);
        if (item == null)
        {
            return -1;
        }

        var count = await InsertMail(
            playerId,
            Id,
            tbl[player.HowLongDays].ItemId,
            "운영자",
            Int32.Parse(tbl[player.HowLongDays].Amount),
            player.HowLongDays + "일차 출석 환영해요!",
            "접속 자주 해주세용",
            tbl[player.HowLongDays].ItemName,
            tbl[player.HowLongDays].ItemType);

        return count;
    }

    public async Task<List<Mail>> SelectMail(string recvId)
    {
        var multi = await _connection.QueryMultipleAsync(
            @"SELECT * FROM com2us.mail WHERE PlayerID=@id and IsDeleted=0",
            new { id = recvId }).ConfigureAwait(false);

        var mails = multi.Read<Mail>().ToList();

        return mails;
    }

    public async Task<Int32> DeleteMails(string recvId)
    {
        var count = await _connection.ExecuteAsync(
            @"UPDATE com2us.mail SET IsDeleted=1 WHERE PlayerID=@ID",
            new { ID = recvId });

        return count;
    }

    public async Task<int> InsertItemToInventory(Int32 playerId, string? itemId, int amount, string itemName, string itemType)
    {
        var count = await _connection.ExecuteAsync(
            @"INSERT com2us.inventory(PlayerID, ItemID, Amount, ItemName, ItemType) VALUES(@_playerId, @_itemId, @_amount, @_itemName, @_itemType)",
            new { _playerId = playerId, _itemId = itemId, _amount = amount, _itemName = itemName, _itemType = itemType});

        return count;
    }

    public async Task<List<Inventory>> SelectInventoryList(string playerId)
    {
        var multi = await _connection.QueryMultipleAsync(
            @"SELECT * FROM com2us.inventory WHERE PlayerID=@_playerId",
            new { _playerId = playerId }).ConfigureAwait(false);

        var items = multi.Read<Inventory>().ToList();

        return items;
    }

    public async Task<Item?> SelectItemQuery(string itemId)
    {
        var item = await _connection.QuerySingleOrDefaultAsync<Item>(
            @"SELECT * FROM com2us.item WHERE ItemID=@ItemID",
            new { ItemID = itemId });

        return item;
    }

    public async Task<List<InventoryItem>> SelectAllInventoryItems(string playerId)
    {
        var multi = await _connection.QueryMultipleAsync(
            @"SELECT ItemID, ItemName, ItemType, Amount FROM com2us.Inventory WHERE PlayerID = @_playerId",
            new { _playerId = playerId, }).ConfigureAwait(false);
        var items = multi.Read<InventoryItem>().ToList();

        return items;
    }

    public async Task<int> InsertRobotmon(int monId, string name, string chr, int level, int hp, int att, int def, int star)
    {
        var count = await _connection.ExecuteAsync(
            @"INSERT com2us.robotmon(RobotmonID, Name, Characteristic, Level, HP, Attack, Defense, Star) 
                            VALUES(@_monId, @_name, @_chr, @_level, @_hp, @_att, @_def, @_star)",
            new
            {
                _monId = monId,
                _name = name,
                _chr = chr,
                _level = level,
                _hp = hp,
                _att = att,
                _def = def,
                _star = star
            });
        return count;
    }

    public async Task<int> InsertRobotmonDirect(RobotMon mon)
    {
        var count = await _connection.ExecuteAsync(
            @"INSERT com2us.robotmon(RobotmonID, Name, Characteristic, Level, HP, Attack, Defense, Star) 
                            VALUES(@_monId, @_name, @_chr, @_level, @_hp, @_att, @_def, @_star)",
            new
            {
                _monId = mon.RobotmonID,
                _name = mon.Name,
                _chr = mon.Characteristic,
                _level = mon.Level,
                _hp = mon.HP,
                _att = mon.Attack,
                _def = mon.Defense,
                _star = mon.Star
            });
        return count;
    }

    public async Task<RobotMon?> SelectRobotmon(int monId)
    {
        var mon = await _connection.QuerySingleOrDefaultAsync<RobotMon>(
            @"SELECT * FROM com2us.robotmon WHERE RobotmonID=@_monId",
            new { _monId = monId });
        return mon;
    }

    public async Task<PlayerRobotmon?> SelectPlayerRobotmonQuery(string Id)
    {
        var playerRobotmonInfo = await _connection.QuerySingleOrDefaultAsync<PlayerRobotmon>(
            @"select * from com2us.playerrobotmon where PlayerID=@id",
            new { id = Id });

        return playerRobotmonInfo;
    }

    public async Task<RobotmonUpgrade?> SelectRobotmonUpgrade(Int32 monId)
    {
        var monUpgrade = await _connection.QuerySingleOrDefaultAsync<RobotmonUpgrade>(
            @"SELECT * FROM com2us.robotmon_upgrade WHERE RobotmonID=@_monId",
            new { _monId = monId });
        return monUpgrade;
    }

    public async Task<int> InsertRobotmonUpgradeDirect(RobotmonUpgrade ru)
    {
        var count = await _connection.ExecuteAsync(
            @"INSERT com2us.robotmon_upgrade(RobotmonID, EvolveStar, NextEvolveID, Reinforce1, Reinforce2, Reinforce3) 
                            VALUES(@_monId, @_star, @_nextId, @_rf1, @_rf2, @_rf3)",
            new
            {
                _monId = ru.RobotmonID,
                _star = ru.EvolveStar,
                _nextId = ru.NextEvolveID,
                _rf1 = ru.Reinforce1,
                _rf2 = ru.Reinforce2,
                _rf3 = ru.Reinforce3,
            });
        return count;
    }

    public async Task<int> UpdateRobotmonUpgradeDirect(RobotmonUpgrade ru)
    {
        var count = await _connection.ExecuteAsync(
            @"UPDATE com2us.robotmon_upgrade SET Reinforce1=@_rf1, Reinforce2=@_rf2, Reinforce3=@_rf3 WHERE RobotmonID=@_id",
            new
            {
                _rf1 = ru.Reinforce1,
                _rf2 = ru.Reinforce2,
                _rf3 = ru.Reinforce3,
                _id = ru.RobotmonID,
            });
        return count;
    }

    public async Task<int> RobotmonUpgradeInsertAndUpdate(RobotmonUpgrade ru)
    {
        //있다면 update
        if(null != await SelectRobotmonUpgrade(ru.RobotmonID))
        {
            var updateCnt = await UpdateRobotmonUpgradeDirect(ru);
            return updateCnt;
        }

        //없다면 insert
        var insertCnt = await InsertRobotmonUpgradeDirect(ru);
        return insertCnt;
    }
}
