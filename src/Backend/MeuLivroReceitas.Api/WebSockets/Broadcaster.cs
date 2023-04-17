using System.Collections.Concurrent;
using MeuLivroReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.SignalR;

namespace MeuLivroReceitas.Api.WebSockets;

//Broadcaster usado para compartilhar informacoes
//entre as conexoes ativas
public class Broadcaster
{
    private readonly static Lazy<Broadcaster> _instance = new(() => new Broadcaster());

    public static Broadcaster Instance { get { return _instance.Value; } }

    //armazena os dados que se quer compartilhar
    //_dictionary tem como string o connectionId
    //e com object o id do usuario que gerou o token
    private ConcurrentDictionary<string, object> _dictionary { get; set; }

    public Broadcaster()
    {
        _dictionary = new ConcurrentDictionary<string, object>();
    }

    public void InitializeConnection(
        IHubContext<AddConnection> hubContext,
        string ownerUserQRCode,
        string connectionId
        )
    {
        //connectionId e hubContext criados em AddConnection
        var connection = new Connection(hubContext, connectionId);

        //em Connection esta a funcao que a cada 1s
        // chama uma mensagem para enviar o evento ao cliente
        //em _dictionary contem connectionId e o id do usuario que gerou token
        _dictionary.TryAdd(connectionId, connection);

        //para recuperar o usuario que gerou o token
        _dictionary.TryAdd(ownerUserQRCode, connectionId);

        //StartTimeCount criado em Api.WebSockets Connection
        connection.StartTimeCount(CallbackExpiredTime);
    }

    //usado em Connection em StartTimeCount
    private void CallbackExpiredTime(string connectionId)
    {
        // remover o connectionId expirado do db
        _dictionary.TryRemove(connectionId, out _);
    }

    public string GetConnectionIdUser(string UserId)
    {
        //tenta pegar o valor de UserId e salvar em connectionId
        //se conseguir retorna a variavel como string
        if (!_dictionary.TryGetValue(UserId, out var connectionId))
        {
            throw new ExceptionBase("");
        }

        return connectionId.ToString();
    }

    public void ResetExpirationTime(string connectionId)
    {
        _dictionary.TryGetValue(connectionId, out var objectConnection);

        var connection = objectConnection as Connection;

        connection.ResetTimer();
    }

    //conexao do usuario leitor do qrCode
    public void setConnectionIdUserReader(string ownerQRCodeId, string connectionId)
    {
        var connectionIdReaderUser = GetConnectionIdUser(ownerQRCodeId);

        _dictionary.TryGetValue(connectionId, out var objectConnection);

        var connection = objectConnection as Connection;

        connection.setConnectionIdUserReader(connectionId);
    }

    public string Remove(string connectionId, string userId)
    {
        _dictionary.TryGetValue(connectionId, out var objectConnection);

        var connection = objectConnection as Connection;

        connection.StopTimer();
       
        _dictionary.TryRemove(connectionId, out _);
        _dictionary.TryRemove(userId, out _);

        return connection.UserReadQrCode();
    }
}
