using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Banca.UtilityService.Application;
public class EncryptionService : IEncryptionService
{

    private readonly ILogger<EncryptionService> logger;

    public EncryptionService(ILogger<EncryptionService> logger)

    {

        this.logger = logger;

    }

    public string DecryptAes(string ciphertext, string secretKey, string iv)

    {
        try

        {

            using Aes myAes = Aes.Create();

            myAes.Key = Encoding.UTF8.GetBytes(secretKey);

            myAes.IV = Encoding.UTF8.GetBytes(iv);

            string roundtrip = DecryptStringFromBytes_Aes(ciphertext, myAes.Key, myAes.IV);

            return roundtrip;

        }

        catch (Exception ex)

        {

            logger.LogError(ex, $"Method: DecryptAes. {ciphertext}");

            return string.Empty;

        }

    }



    private static string DecryptStringFromBytes_Aes(string cipherText, byte[] Key, byte[] IV)

    {

        if (cipherText == null || cipherText.Length <= 0)

            throw new ArgumentNullException("cipherText");

        if (Key == null || Key.Length <= 0)

            throw new ArgumentNullException("Key");

        if (IV == null || IV.Length <= 0)

            throw new ArgumentNullException("IV");

        string plaintext = null!;

        using (Aes aesAlg = Aes.Create())

        {

            aesAlg.Key = Key;

            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            byte[] cipherbytes = HexadecimalStringToByteArray(cipherText);

            using MemoryStream msDecrypt = new(cipherbytes);

            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);

            using StreamReader srDecrypt = new(csDecrypt);

            plaintext = srDecrypt.ReadToEnd();

        }

        return plaintext;

    }



    public string EncryptAes(string plaintext, string secretkey, string iv)

    {

        try

        {

            using Aes myAes = Aes.Create();

            myAes.Key = Encoding.UTF8.GetBytes(secretkey);

            myAes.IV = Encoding.UTF8.GetBytes(iv);

            byte[] encrypted = EncryptStringToBytes_Aes(plaintext, myAes.Key, myAes.IV);

            string ciphertext = ByteArrayToString(encrypted);

            return ciphertext;

        }

        catch (Exception ex)

        {

            logger.LogError(ex, $"Method: EncryptAes. {plaintext}");

            return string.Empty;

        }

    }



    private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)

    {

        if (plainText == null || plainText.Length <= 0)

            throw new ArgumentNullException("plainText");

        if (Key == null || Key.Length <= 0)

            throw new ArgumentNullException("Key");

        if (IV == null || IV.Length <= 0)

            throw new ArgumentNullException("IV");

        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())

        {

            aesAlg.Key = Key;

            aesAlg.IV = IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new();

            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);

            using (StreamWriter swEncrypt = new(csEncrypt))

            {

                swEncrypt.Write(plainText);

            }

            encrypted = msEncrypt.ToArray();

        }

        return encrypted;

    }

    private static byte[] HexadecimalStringToByteArray(string input)

    {

        var outputLength = input.Length / 2;

        var output = new byte[outputLength];

        using (var sr = new StringReader(input))

        {

            for (var i = 0; i < outputLength; i++)

                output[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);

        }

        return output;

    }



    private static string ByteArrayToString(byte[] ba)

    {

        StringBuilder hex = new(ba.Length * 2);

        foreach (byte b in ba) hex.AppendFormat("{0:x2}", b); return hex.ToString();

    }

}
