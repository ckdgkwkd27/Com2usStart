using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace com2us_start;

public class Crypter
{
    private static byte[] _key;
    private static byte[] _iv;
    private Crypter()
    {
        byte[] DEF_RIJNDAEL_IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        string key = "com2uscryptokey";

        _key = Encoding.UTF8.GetBytes(key);
        _iv = DEF_RIJNDAEL_IV;
    }
    //Singleton
    private static readonly Lazy<Crypter> _instance = new Lazy<Crypter>(() => new Crypter());
    public static Crypter Instance { get { return _instance.Value; } }
    //

    static string EncryptStringToBytes(string plainText)
    {
        byte[] encrypted;

        using (Rijndael rijAlg = Rijndael.Create())
        {
            rijAlg.Key = _key;
            rijAlg.IV = _iv;

            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {

                        streamWriter.Write(plainText);
                    }
                    encrypted = memoryStream.ToArray();
                }
            }
        }
            
        return Convert.ToBase64String(encrypted);
    }

    static string DecryptStringFromBytes(string cipherText)
    {
        string plaintext = null;
            
        using (Rijndael rijAlg = Rijndael.Create())
        {
            rijAlg.Key = _key;
            rijAlg.IV = _iv;

            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                
            using (MemoryStream memoryStream = new MemoryStream(Convert.ToByte(cipherText)))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        plaintext = streamReader.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }
}
