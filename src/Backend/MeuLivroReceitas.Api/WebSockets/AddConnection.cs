using MeuLivroReceitas.Application.UseCases.Connection.acceptConnection;
using MeuLivroReceitas.Application.UseCases.Connection.GenerateQRCode;
using MeuLivroReceitas.Application.UseCases.Connection.ReadQRCode;
using MeuLivroReceitas.Application.UseCases.Connection.RefuseConnection;
using MeuLivroReceitas.Exceptions;
using MeuLivroReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MeuLivroReceitas.Api.WebSockets;

//como o controller que herda de controllerBase
//um websocket herda de Hub de SignalR

//em program configurar o SignalR

//configurar a Policy em program
//somente usuarios logados acessar o websocket todas funcoes
// Policy indica nome da funcao para autenticacao customizada
[Authorize(Policy = "LoggedUser")]
public class AddConnection: Hub
{
    //Broadcaster utilizado para informar o tempo de expiracao do token
    // em tempo real diminui cada segungo
    //e envia ao usuario
    private readonly Broadcaster _broadcaster;

    private readonly IQRCodeGeneratorUseCase _qrCodeGeneratorUseCase;

    private readonly IQRCodeReadUseCase _qrCodeReadUseCase;
 
    private readonly IRefuseConnectionUseCase _refuseConnectionUseCase;

    private readonly IHubContext<AddConnection> _hubContext;

    private readonly IAcceptConnectionUseCase _acceptConnectionUseCase;
    public AddConnection(
        IHubContext<AddConnection> hubContext,
        IQRCodeGeneratorUseCase qrCodeGenerator,
        IQRCodeReadUseCase qrCodeReadUseCase,
        IRefuseConnectionUseCase refuseConnectionUseCase,
        IAcceptConnectionUseCase acceptConnectionUseCase
        )
    {
        //gerar uma instancia do Broadcaster
        _broadcaster = Broadcaster.Instance;

        _qrCodeGeneratorUseCase = qrCodeGenerator;

        //verificar se o usuario leu o qrCode
        //e retorna o nome do usuario que leu
        _qrCodeReadUseCase = qrCodeReadUseCase;

        _refuseConnectionUseCase = refuseConnectionUseCase;

        _acceptConnectionUseCase = acceptConnectionUseCase;

        //conexao do hubContext vem do program
        _hubContext = hubContext;
    }

    public async Task GetQRCode()
    {

        try
        {

            (var qrCode, var userId) = await _qrCodeGeneratorUseCase.Execute();

            //inicializar a contagem do tempo
            _broadcaster.InitializeConnection(_hubContext, userId, Context.ConnectionId);
        
            //enviar msg para todos que estao conectados
            //ResultadoQRCode nome da funcao qrCode é o parametro
            //await Clients.All.SendAsync("ResultadoQRCode", qrCode);

            //enviar msg para uma conexao especifica
            //ConnectionId precisa ser associado a um usuario
            //await Clients
            //    .Client(Context.ConnectionId)
            //    .SendAsync("ResultadoQRCode", qrCode);

            //enviar msg para grupos
            //await Groups.AddToGroupAsync(Context.ConnectionId, "nomeGrupo");
            //await Clients
            //    .Group("nomeGrupo")
            //    .SendAsync("ResultadoQRCode", qrCode);

            //devolver msg para quem chamou
            await Groups.AddToGroupAsync(Context.ConnectionId, "nomeGrupo");
            await Clients
                .Caller
                .SendAsync("ResultadoQRCode", qrCode);
        }
        //se for um erro tratado retorna para o usuario o erro
        catch (ExceptionBase ex)
        {
            await Clients.Caller.SendAsync("Erro", ex.Message);
        }
        //se o erro nao for tratado retorna erro desconhecido
        catch
        {
            await Clients.Caller.SendAsync("Erro", ResourceMessageError.UNKNOWN_ERROR);
        }
    }

    //sobrescrever metodo de hub OnConnectedAsync
    //que emite um evento quando uma coneccao é estabelecida
    public override Task OnConnectedAsync()
    {

        //cada conexao gera um id diferente
        //que é utilizado para enviar msg a um grupo
        var x = Context.ConnectionId;
        
        return base.OnConnectedAsync();
    }

    public async Task QRCodeRead(string connectionCode)
    {
        try
        {
            (var userToConnect, var ownerQRCodeId) = await _qrCodeReadUseCase.Execute(connectionCode);

            //_broadcaster devolver o id da conexao do usuario que gerou o qrcode
            var connectionId = _broadcaster.GetConnectionIdUser(ownerQRCodeId);

            _broadcaster.ResetExpirationTime(connectionId);
            _broadcaster.setConnectionIdUserReader(ownerQRCodeId, Context.ConnectionId);

            //devolver para o usuario que criou o qrcode
            //o usuario que leu o qrcode
            //utiliza a connectionId do usuario que gerou o qrCode
            await Clients.Client(connectionId).SendAsync("ResultQRCodeRead", userToConnect);
        }
        //se for um erro tratado retorna para o usuario o erro
        catch (ExceptionBase ex)
        {
            await Clients.Caller.SendAsync("Erro", ex.Message);
        }
        //se o erro nao for tratado retorna erro desconhecido
        catch
        {
            await Clients.Caller.SendAsync("Erro", ResourceMessageError.UNKNOWN_ERROR);
        }

    }

    public async Task RefuseConnection()
    {
        try
        {
            //conexao id do usuario que gerou o qrCode
            var connectionIdOwnerQRCode = Context.ConnectionId;

            var userId = await _refuseConnectionUseCase.Execute();

            var connetionIdReaderUser = _broadcaster.Remove(connectionIdOwnerQRCode, userId);

            //devolver para o usuario que leu o qrCode que
            //a conexao dele foi recusada
            await Clients.Client(connetionIdReaderUser).SendAsync("OnConnectionRefused");
        }
        catch (ExceptionBase ex)
        {
            await Clients.Caller.SendAsync("Erro", ex.Message);
        }
        //se o erro nao for tratado retorna erro desconhecido
        catch
        {
            await Clients.Caller.SendAsync("Erro", ResourceMessageError.UNKNOWN_ERROR);
        }
    }

    public async Task AceptConnection(string userIdToConnect)
    {
        try
        {
            var userId = await _acceptConnectionUseCase.Execute(userIdToConnect);

            //conexao id do usuario que gerou o qrCode
            var connectionIdOwnerQRCode = Context.ConnectionId;

            var connetionIdReaderUser = _broadcaster.Remove(connectionIdOwnerQRCode, userId);

            //devolver para o usuario que leu o qrCode que
            //a conexao dele foi aceita
            await Clients.Client(connetionIdReaderUser).SendAsync("OnConnectionAccepted");
        }
        catch (ExceptionBase ex)
        {
            await Clients.Caller.SendAsync("Erro", ex.Message);
        }
        //se o erro nao for tratado retorna erro desconhecido
        catch
        {
            await Clients.Caller.SendAsync("Erro", ResourceMessageError.UNKNOWN_ERROR);
        }
    }
}
