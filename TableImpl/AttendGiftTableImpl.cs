using static System.Int32;

namespace com2us_start.TableImpl;

public class AttendGiftTableImpl : ICsvTableBase
{
    public static Dictionary<Int32, AttendGiftTableMember> GiftDict;

    public AttendGiftTableImpl()
    {
        GiftDict = new Dictionary<Int32, AttendGiftTableMember>();
    }

    public bool Execute(string filename)
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

    public void Load()
    { 
        foreach (var tbl in GiftDict)
        {
            //Console.Write(tbl.Days + " ");
            Console.Write(tbl.Key + " ");
            Console.Write(tbl.Value.ItemName + " ");
            Console.Write(tbl.Value.ItemId + " ");
            Console.Write(tbl.Value.ItemType + " ");
            Console.Write(tbl.Value.Amount + " ");
            Console.Write("\n");
        }
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