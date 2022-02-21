using com2us_start.TableImpl;

namespace com2us_start;

public class CsvTableLoader
{
    private CsvTableLoader()
    {
        registeredTable = new Dictionary<string, ICsvTableBase>();

        registeredTable.Add("AttendGift.csv", new AttendGiftTableImpl());
    }

    //Singleton
    private static readonly Lazy<CsvTableLoader> _instance = new Lazy<CsvTableLoader>(() => new CsvTableLoader());
    public static CsvTableLoader Instance { get { return _instance.Value; } }

    private Dictionary<string, ICsvTableBase> registeredTable;

    public void Load()
    {
        foreach (var table in registeredTable)
        {
            table.Value.Execute(table.Key);
            //table.Value.Load();
        }
    }
    
}
