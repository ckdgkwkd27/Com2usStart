
public enum ErrorCode : int
{
    None = 0,

    Join_Fail_Duplicate = 11,
    Join_Fail_Exception = 12,
    Join_Fail_PlayerFailed = 13,

    Login_Fail_NotUser = 21,
    Login_Fail_Exception = 22,

    Load_Fail_NotUser = 31,
    Load_Fail_Exception = 32,
    Load_Fail_NotAuthorized = 33,
}

