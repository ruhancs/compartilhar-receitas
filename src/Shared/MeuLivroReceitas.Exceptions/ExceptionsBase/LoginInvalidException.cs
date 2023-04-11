using System.Runtime.Serialization;

namespace MeuLivroReceitas.Exceptions.ExceptionsBase;

//resolver code smell do sonar
[Serializable]
public class LoginInvalidException : ExceptionBase
{
    public LoginInvalidException() : base(ResourceMessageError.LOGIN_INVALID)
    {
        
    }
    //resolver code smell do sonar
    protected LoginInvalidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
