namespace com2us_start.TableImpl;

public class EvolutionInfoTableImpl : ICsvTableBase
{
    public static List<RobotmonUpgrade> sRobotmonUpgradeList;
    private IRealDbConnector _realDbConnector;


    public EvolutionInfoTableImpl(IRealDbConnector realDbConnector)
    {
        sRobotmonUpgradeList = new List<RobotmonUpgrade>();
        _realDbConnector = realDbConnector;
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
            RobotmonUpgrade ru = new RobotmonUpgrade();
            ru.RobotmonID = Int32.Parse(list[0]);
            ru.EvolveStar = Int32.Parse(list[1]);
            ru.NextEvolveID = Int32.Parse(list[2]);

            try
            {
                var count = await _realDbConnector.RobotmonUpgradeInsertAndUpdate(ru);
                if (count != 1)
                {
                    Console.WriteLine("Upgrade Info Query Error!");
                    continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                continue;
            }
            sRobotmonUpgradeList.Add(ru);
            ++i;
        }
        
        return true;
    }
}
