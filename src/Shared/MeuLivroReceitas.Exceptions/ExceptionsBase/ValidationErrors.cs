using System.Runtime.Serialization;

namespace MeuLivroReceitas.Exceptions.ExceptionsBase;

//resolver code smell do sonar
[Serializable]
public class ValidationErrors : ExceptionBase
{
    public List<string> MessagesErro { get; set; }

    public ValidationErrors(List<string> messagesErro) : base(String.Empty)
    {
        MessagesErro = messagesErro;
    }

    //resolver code smell do sonar
    protected ValidationErrors(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
