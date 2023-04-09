namespace MeuLivroReceitas.Exceptions.ExceptionsBase;

public class LoginInvalidException : ExceptionBase
{
    public LoginInvalidException() : base(ResourceMessageError.LOGIN_INVALID)
    {
        
    }
}
