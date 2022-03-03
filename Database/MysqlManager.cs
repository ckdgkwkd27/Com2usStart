using com2us_start.TableImpl;
using Dapper;
using MySqlConnector;

namespace com2us_start;

public class MysqlManager : IDisposable
{
    private readonly IRealDbConnector _realDbConnector;

    public MysqlManager(IConfiguration conf, IRealDbConnector realDbConnector)
    {
        _realDbConnector = realDbConnector;
        _realDbConnector.InitConnector(conf);
    }

    //Dispose Pattern
    ~MysqlManager()
    {
        Dispose();
    }

    public async Task<MySqlConnection> GetDbConnection()
    {
        return await _realDbConnector.Connect();
    }

    public async void Dispose()
    {
        Console.WriteLine("Disposing........................................................................");
        await _realDbConnector.Disconnect();
        _realDbConnector.Dispose();
    }

    //Query를 내부에서 처리 

    //Member
    public async Task<Int32> InsertAccountQuery(string id, string password, string salt)
    {
        return await _realDbConnector.InsertAccountQuery(id, password, salt);
    }

    public async Task<Member?> SelectMemberQuery(string Id)
    {
        return await _realDbConnector.SelectMemberQuery(Id);
    }

    //GamePlayer
    public async Task<Int32> InsertPlayer(string playerId, string id, Int32 level, Int32 exp, Int32 gameMoney)
    {
        return await _realDbConnector.InsertPlayer(playerId, id, level, exp, gameMoney);
    }

    public async Task<GamePlayer?> SelectGamePlayerQuery(string Id)
    {
        return await _realDbConnector.SelectGamePlayerQuery(Id);
    }

    public async Task<Int32> UpdatePlayerAttend(string Id, bool isGiftGiven = false)
    {
        return await _realDbConnector.UpdatePlayerAttend(Id, isGiftGiven);
    }

    //Mail
    public async Task<Int32> InsertMail(string playerId, string recvId, string? itemId, string sendName, int amount, 
        string title, string content, string itemName, string itemType, Int32 money = 0)
    {
        return await _realDbConnector.InsertMail(playerId, recvId, itemId, sendName, amount, title, content, itemName, itemType, money);
    }

    public async Task<Int32> InsertAttendOperationMail(string playerId, string? Id, GamePlayer player)
    {
        return await _realDbConnector.InsertAttendOperationMail(playerId, Id, player);
    }

    public async Task<List<Mail>> SelectMultipleMailQuery(string recvId)
    {
        return await _realDbConnector.SelectMultipleMailQuery(recvId);
    }

    public async Task<Int32> DeleteMails(string recvId)
    {
        return await _realDbConnector.DeleteMails(recvId);
    }

    //Inventory
    public async Task<Int32> InsertItemToInventory(string playerId, string? itemId, int amount, string itemName, string itemType)
    {
        return await _realDbConnector.InsertItemToInventory(playerId, itemId, amount, itemName, itemType);
    }

    public async Task<List<Inventory>> SelectMultipleInventoryQuery(string playerId)
    {
        return await _realDbConnector.SelectMultipleInventoryQuery(playerId);
    }

    //Item
    public async Task<Item?> SelectItemQuery(string itemId)
    {
        return await _realDbConnector.SelectItemQuery(itemId);
    }

    public async Task<List<InventoryItem>> SelectAllInventoryItems(string playerId)
    {
        return await _realDbConnector.SelectAllInventoryItems(playerId);
    }

    //Robotmon
    public async Task<Int32> InsertRobotmon(int monId, string name, string chr, Int32 level, Int32 hp, Int32 att,
        Int32 def, Int32 star)
    {
        return await _realDbConnector.InsertRobotmon(monId, name, chr, level, hp, att, def, star);
    }

    public async Task<Int32> InsertRobotmonDirect(RobotMon mon)
    {
        return await _realDbConnector.InsertRobotmonDirect(mon);
    }

    public async Task<RobotMon?> SelectRobotmonQuery(Int32 monId)
    {
        return await _realDbConnector.SelectRobotmonQuery(monId);
    }

    //PlayerRobotmon
    public async Task<PlayerRobotmon?> SelectPlayerRobotmonQuery(string Id)
    {
        return await _realDbConnector.SelectPlayerRobotmonQuery(Id);
    }
    
    //Robotmon_Upgrade
    public async Task<RobotmonUpgrade?> SelectRobotmonUpgrade(Int32 monId)
    {
        return await _realDbConnector.SelectRobotmonUpgrade(monId);
    }
    public async Task<Int32> InsertRobotmonUpgradeDirect(RobotmonUpgrade ru)
    {
        return await _realDbConnector.InsertRobotmonUpgradeDirect(ru);
    }

    public async Task<Int32> UpdateRobotmonUpgradeDirect(RobotmonUpgrade ru)
    {
        return await _realDbConnector.UpdateRobotmonUpgradeDirect(ru);
    }

    public async Task<Int32> RobotmonUpgradeInsertAndUpdate(RobotmonUpgrade ru)
    {
        return await _realDbConnector.RobotmonUpgradeInsertAndUpdate(ru);
    }
}
