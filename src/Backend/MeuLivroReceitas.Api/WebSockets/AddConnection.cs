using System.Security.Cryptography.X509Certificates;
using MeuLivroReceitas.Application.UseCases.Connection.QRCodeGenerator;
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
    private readonly IQRCodeGeneratorUseCase _qrCodeGeneratorUseCase;

    public AddConnection(IQRCodeGeneratorUseCase qrCodeGenerator)
    {
        _qrCodeGeneratorUseCase = qrCodeGenerator;
    }

    public async Task GetQRCode()
    {
        var qrCode = await _qrCodeGeneratorUseCase.Execute();

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

    //sobrescrever metodo de hub OnConnectedAsync
    //que emite um evento quando uma coneccao é estabelecida
    public override Task OnConnectedAsync()
    {

        //cada conexao gera um id diferente
        //que é utilizado para enviar msg a um grupo
        var x = Context.ConnectionId;
        
        return base.OnConnectedAsync();
    }
}
