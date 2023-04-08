using System.Security.Cryptography;
using System.Text;

namespace MeuLivroReceitas.Application.Services.Cryptography;

public class EncryptPassword
{
        private readonly string _encryptyKey;

    public EncryptPassword(string encryptyKey)
    {
        _encryptyKey = encryptyKey;
    }
    public string Cryptography(string password)
    {
        var passwordWitchencryptKey = $"{password}{_encryptyKey}";

        var bytes = Encoding.UTF8.GetBytes(passwordWitchencryptKey);
        var sha512 = SHA256.Create();
        byte[] hashBytes = sha512.ComputeHash(bytes);
        return StringBytes(hashBytes);
    }

    private static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }
        return sb.ToString();
    }
}
