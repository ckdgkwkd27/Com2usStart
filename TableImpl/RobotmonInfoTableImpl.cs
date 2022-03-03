namespace com2us_start.TableImpl;

public class RobotmonInfoTableImpl : ICsvTableBase
{
    public static List<RobotMon> sRobotmonList;
    private IRealDbConnector _realDbConnector;

    public RobotmonInfoTableImpl(IRealDbConnector realDbConnector)
    {
        sRobotmonList = new List<RobotMon>();
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
            RobotMon robotMon = new RobotMon();
            robotMon.RobotmonID = Int32.Parse(list[0]);
            robotMon.Name = list[1];
            robotMon.Characteristic = list[2];
            robotMon.Level = Int32.Parse(list[3]);
            robotMon.HP = Int32.Parse(list[4]);
            robotMon.Attack = Int32.Parse(list[5]);
            robotMon.Defense = Int32.Parse(list[6]);
            robotMon.Star = Int32.Parse(list[7]);
            try
            {
                var rb = await manager.SelectRobotmonQuery(robotMon.RobotmonID);
                if (rb != null)
                {
                    continue;
                }
                
                var count = await manager.InsertRobotmonDirect(robotMon);
                if (count != 1)
                {
                    Console.WriteLine("Insert RobotMon Error!");
                    continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                continue;
            }

            sRobotmonList.Add(robotMon);
            ++i;
        }

        return true;
    }
}
