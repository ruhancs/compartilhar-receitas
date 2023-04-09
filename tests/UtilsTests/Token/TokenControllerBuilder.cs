using MeuLivroReceitas.Application.Services.Token;

namespace UtilsTests.Token
{
    public class TokenControllerBuilder
    {
        public static TokenController Instance()
        {
            //TokenController recebe tempo de validacao e chave de segurançao
            //chave deve ser base64 de 30 caracteres
            return new TokenController(10, "QU9PUkAkV0QqJDIxdGs1XlgjVUF2NHJXZ2p1Yk81");
        }
    }
}
