namespace com2us_start.TableImpl;

public class ReinforceInfoTableImpl : ICsvTableBase
{
    //일단 출력하고 이후에 테이블 까서 db에 넣자

    public static List<RobotmonUpgrade> sRobotmonUpgradeList;
    private IRealDbConnector _realDbConnector;


    public ReinforceInfoTableImpl(IRealDbConnector realDbConnector)
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
        using MysqlManager manager = new MysqlManager(conf, _realDbConnector);

        foreach (var list in tableList)
        {
            RobotmonUpgrade ru = new RobotmonUpgrade();
            ru.RobotmonID = Int32.Parse(list[0]);
            ru.Reinforce1 = Int32.Parse(list[1]);
            ru.Reinforce2 = Int32.Parse(list[2]);
            ru.Reinforce3 = Int32.Parse(list[3]);

            try
            {
                var count = await manager.RobotmonUpgradeInsertAndUpdate(ru);
                if (count != 1)
                {
                    Console.WriteLine("Upgrade Info Query Error!");
                    return false;
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
