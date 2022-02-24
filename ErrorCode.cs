
public enum ErrorCode : Int32
{
    None = 0,
    
    Token_Fail_NotAuthorized = 1,
    
    Join_Fail_Duplicate = 11,
    Join_Fail_Exception = 12,

    Login_Fail_NotUser = 21,
    Login_Fail_Exception = 22,

    Load_Fail_NotUser = 31,
    Load_Fail_Exception = 32,
    
    Attend_Fail_NotUser = 41,
    Attend_Fail_Exception = 42,
    
    Mail_Fail_Empty = 51,
    Mail_Fail_Exception = 52,
    Mail_Fail_CannotSend = 53,
    
    Recv_Fail_NotUser = 61,
    Recv_Fail_Exception = 62,
    Recv_Fail_InvalidRecv = 63,
    
    Player_Fail_NotUser = 71,
    Player_Fail_Exception = 72,
    Player_Fail_Insertion = 73,

    Inventory_Fail_Empty = 81,
    Inventory_Fail_Exception = 82,
}

