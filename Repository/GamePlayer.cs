namespace com2us_start;

public class GamePlayer
{
    public string UUID { get; set; }
    public string ID { get; set; }
    public Int32 Level { get; set; }
    public Int32 Exp { get; set; }
    public Int32 GameMoney { get; set; }
    
    public DateTime AttendDate { get; set; }
        
    //보상받은 날짜
    public DateTime GiftDate { get; set; }
    
    //며칠동안 출석했나요
    public Int32 HowLongDays { get; set; }
}
