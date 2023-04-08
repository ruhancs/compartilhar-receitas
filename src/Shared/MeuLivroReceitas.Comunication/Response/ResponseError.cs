namespace MeuLivroReceitas.Comunication.Response;

public class ResponseError
{
    public List<string> Messages { get; set; }

    //duas formas de pegar as mensagens
    public ResponseError(string messages)
    {
        Messages = new List<string>();
        Messages.Add(messages);
    }
    public ResponseError(List<string> messages)
    {
        Messages = messages;
    }
}
