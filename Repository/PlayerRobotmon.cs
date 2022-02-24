namespace com2us_start;

//플레이어가 잡은 로봇몬
public class PlayerRobotmon
{
    public string UUID { get; set; }
    public string PlayerID { get; set; }
    public string RobotmonID { get; set; }
    public Int32 Level { get; set; }
    public Int32 HP { get; set; }
    public Int32 Star { get; set; }
    public Int32 CatchedLocX { get; set; }
    public Int32 CatchedLocY { get; set; }
    public Int32 Reinforcement { get; set; }
    public DateTime CatchedDate { get; set; }
}
