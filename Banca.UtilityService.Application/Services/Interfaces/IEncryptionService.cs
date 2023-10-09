namespace Banca.UtilityService.Application;
public interface IEncryptionService
{
    string DecryptAes(string ciphertext, string secretKey, string iv);
    string EncryptAes(string plaintext, string secretkey, string iv);
}
