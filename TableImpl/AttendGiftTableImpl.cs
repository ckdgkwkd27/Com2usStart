using static System.Int32;

namespace com2us_start.TableImpl;

public class AttendGiftTableImpl : ICsvTableBase
{
    public static Dictionary<Int32, AttendGiftTableMember> GiftDict;

    public AttendGiftTableImpl()
    {
        GiftDict = new Dictionary<Int32, AttendGiftTableMember>();
    }

    public async Task<bool> ExecuteAsync(string filename, IConfiguration conf)
    {
        var tableList = CsvParser.Instance.Parse(filename);
        if (tableList.Count < 1)
        {
            return false;
        }

        Int32 i = 0;
        foreach (var list in tableList)
        {
            AttendGiftTableMember tbl = new AttendGiftTableMember();
            //tbl.Days = list[0];
            tbl.ItemName = list[1];
            tbl.ItemId = list[2];
            tbl.ItemType = list[3];
            tbl.Amount = list[4];
            GiftDict.Add(i, tbl);
            ++i;
        }
        
        return true;
    }
}

public class AttendGiftTableMember
{
    //public string? Days { get; set; }
    public string? ItemName { get; set; }
    public string? ItemId { get; set; }
    public string? ItemType { get; set; }
    public string? Amount { get; set; }
}