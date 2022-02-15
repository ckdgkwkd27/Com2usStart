namespace com2us_start;

//플레이어가 잡은 로봇몬
public class PlayerRobotmon
{
    public string PlayerID { get; set; }
    public string RobotmonID { get; set; }
    public int Level { get; set; }
    public int HP { get; set; }
    public int Star { get; set; }
    public int CatchedLocX { get; set; }
    public int CatchedLocY { get; set; }
    public int Reinforcement { get; set; }
    //DateTime으로도?
    public string CatchedDate { get; set; }
}
