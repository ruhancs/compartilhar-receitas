using System.Runtime.Serialization;

namespace MeuLivroReceitas.Exceptions.ExceptionsBase;

//resolver code smell do sonar
[Serializable]
public class ExceptionBase: SystemException
{
    public ExceptionBase(string message) : base(message)
    {
    }

    //resolver code smell do sonar
    protected ExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
