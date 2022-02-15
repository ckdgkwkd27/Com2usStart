using System.Security.Cryptography;
using CloudStructures;

namespace com2us_start;

public class RedisManager
{
    private string RedisAddress;
    private const string allowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";

    //Singleton
    private RedisManager() { }
    private static readonly Lazy<RedisManager> _instance = new Lazy<RedisManager>(() => new RedisManager());
    public static RedisManager Instance { get { return _instance.Value; } }
    
    public RedisConnection RedisConn { get; set; }
    
    public void Init(IConfiguration conf)
    {
        RedisAddress = conf.GetSection("DBConnection")["Redis"];

        var config = new RedisConfig("com2us", "127.0.0.1");
        RedisConn = new RedisConnection(config);
    }
    
    public string AuthToken()
    {
        var bytes = new byte[25];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
        }
        return new string(bytes.Select(x => allowableCharacters[x % allowableCharacters.Length]).ToArray());
    }
}
