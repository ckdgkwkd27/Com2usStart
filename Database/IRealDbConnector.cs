using MySqlConnector;

namespace com2us_start;

public interface IRealDbConnector : IDisposable
{
    void Connect();
    Task Disconnect();
    
    //Account
    Task<Int32> InsertAccountQuery(string id, string password, string salt);
    Task<Member?> SelectMember(string Id);
    
    //Game Player
    Task<Int32> InsertPlayer(string id, Int32 level, Int32 exp, Int32 gameMoney);
    Task<GamePlayer?> SelectGamePlayer(string Id);
    Task<Int32> UpdatePlayerAttend(string Id, bool isGiftGiven = false);
    
    //Mail
    Task<Int32> InsertMail(string playerId, string recvId, string? itemId, string sendName, int amount,
        string title, string content, string itemName, string itemType, Int32 money = 0);
    Task<Int32> InsertAttendOperationMail(string playerId, string? Id, GamePlayer player);
    Task<List<Mail>> SelectMail(string recvId);
    Task<Int32> DeleteMails(string recvId);
    
    //Inventory
    Task<Int32> InsertItemToInventory(Int32 playerId, string? itemId, int amount, string itemName, string itemType);
    Task<List<Inventory>> SelectInventoryList(string playerId);
    
    //Item
    Task<Item?> SelectItemQuery(string itemId);
    Task<List<InventoryItem>> SelectAllInventoryItems(string playerId);
    
    //Robotmon
    Task<Int32> InsertRobotmon(int monId, string name, string chr, Int32 level, Int32 hp, Int32 att,
        Int32 def, Int32 star);
    Task<Int32> InsertRobotmonDirect(RobotMon mon);
    Task<RobotMon?> SelectRobotmon(Int32 monId);
    
    //Player Robotmon
    Task<PlayerRobotmon?> SelectPlayerRobotmonQuery(string Id);
    
    //Robotmon Upgrade
    Task<RobotmonUpgrade?> SelectRobotmonUpgrade(Int32 monId);
    Task<Int32> InsertRobotmonUpgradeDirect(RobotmonUpgrade ru);
    Task<Int32> UpdateRobotmonUpgradeDirect(RobotmonUpgrade ru);
    Task<Int32> RobotmonUpgradeInsertAndUpdate(RobotmonUpgrade ru);
}
