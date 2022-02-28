using com2us_start.TableImpl;

namespace com2us_start;

public class CsvTableLoader
{
    private CsvTableLoader()
    {
        registeredTable = new Dictionary<string, ICsvTableBase>();
    }

    //Singleton
    private static readonly Lazy<CsvTableLoader> _instance = new Lazy<CsvTableLoader>(() => new CsvTableLoader());
    public static CsvTableLoader Instance { get { return _instance.Value; } }

    private Dictionary<string, ICsvTableBase> registeredTable;
    private IConfiguration _conf;
    private async Task Load()
    {
        foreach (var table in registeredTable)
        {
            var ret = await table.Value.ExecuteAsync(table.Key, _conf);
            //table.Value.Load();
        }
    }
    
    public async Task Init(IConfiguration conf)
    {
        _conf = conf;
        RealDbConnector realDbConnector = new RealDbConnector();
        
        registeredTable.Add("AttendGift.csv", new AttendGiftTableImpl());
        registeredTable.Add("RobotmonInfo.csv", new RobotmonInfoTableImpl(realDbConnector));
        registeredTable.Add("EvolutionInfo.csv", new EvolutionInfoTableImpl(realDbConnector));
        registeredTable.Add("ReinforceInfo.csv", new ReinforceInfoTableImpl(realDbConnector));
        
        await Load();
    }
}
