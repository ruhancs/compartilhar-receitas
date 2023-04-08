namespace MeuLivroReceitas.Exceptions.ExceptionsBase;

public class ValidationErrors : ExceptionBase
{
    public List<string> MessagesErro { get; set; }

    public ValidationErrors(List<string> messagesErro)
    {
        MessagesErro = messagesErro;
    }
}
