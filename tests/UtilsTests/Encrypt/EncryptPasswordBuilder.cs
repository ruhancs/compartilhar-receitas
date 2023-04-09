using MeuLivroReceitas.Application.Services.Cryptography;

namespace UtilsTests.Encrypt;

public class EncryptPasswordBuilder
{
    public static EncryptPassword Intance()
    {
        return new EncryptPassword("12345678");
    }
}
