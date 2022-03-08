namespace com2us_start;

public class Mail
{
    public Int32 PlayerID { get; set; }
    public string MailID { get; set; }
    public string? ItemID { get; set; }
    public string? RecvID { get; set; }
    public string SendName { get; set; }
    public DateTime SendDate { get; set; }
    public Int32 Amount { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Int32 Money { get; set; }
    public string ItemName { get; set; }
    public string ItemType { get; set; }
    private bool IsDeleted { get; set; } 
}
