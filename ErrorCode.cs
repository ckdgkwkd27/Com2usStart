
public enum ErrorCode : Int32
{
    None = 0,
    
    Token_Fail_NotAuthorized = 1,
    
    Join_Fail_Duplicate = 11,
    Join_Fail_Exception = 12,
    Join_Fail_PlayerFailed = 13,

    Login_Fail_NotUser = 21,
    Login_Fail_Exception = 22,

    Load_Fail_NotUser = 31,
    Load_Fail_Exception = 32,
    
    Attend_Fail_NotUser = 41,
    Attend_Fail_Exception = 42,
    
    Mail_Fail_Empty = 51,
    Mail_Fail_Exception = 52,
    Mail_Fail_CannotSend = 53,
}

