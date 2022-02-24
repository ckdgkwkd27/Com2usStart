using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace com2us_start;

public class CryptoManager
{
    private const string allowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";

    private CryptoManager() { }

    //Singleton
    private static readonly Lazy<CryptoManager> _instance = new Lazy<CryptoManager>(() => new CryptoManager());
    public static CryptoManager Instance { get { return _instance.Value; } }
    //

    public string MakeHashingPassword(string saltValue, string pwd)
    {
        var sha = new SHA256Managed();
        byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(saltValue + pwd));
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in hash)
        {
            //16바이트 포맷으로 2글자씩
            stringBuilder.AppendFormat("{0:x2}", b);
        }

        return stringBuilder.ToString();
    }
        
    public string SaltString()
    {
        var bytes = new byte[64];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
        }
        return new string(bytes.Select(x => allowableCharacters[x % allowableCharacters.Length]).ToArray());
    }
}
