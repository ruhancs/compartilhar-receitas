using System.Timers;
using Microsoft.AspNetCore.SignalR;
using Timer = System.Timers.Timer;

namespace MeuLivroReceitas.Api.WebSockets;

public class Connection
{
    private readonly IHubContext<AddConnection> _hubContext;

    //id da conxao gerado em AddConnection
    private readonly string _qrCodeOwnerConnectionId;

    //recebe a funcao em Broadcaster CallbackExpiredTime
    private Action<string> _callbackExpiredTime;

    //id da conexao do usuario que leu o qrCode
    private string ConnectionIdUserReadQRCode;
    public Connection(
        IHubContext<AddConnection> hubContext,
        string qrCodeOwnerConnectionId
        )
    {
        _qrCodeOwnerConnectionId = qrCodeOwnerConnectionId;
        _hubContext = hubContext;
    }

    private short remainingTimeInSeconds { get; set; }
    private Timer _timer { get; set; }

    public void StartTimeCount(Action<string> callbackExpiredTime)
    {
        _callbackExpiredTime = callbackExpiredTime;

        StartTimer();

    }

    public void ResetTimer()
    {
        StopTimer();

        StartTimer();
    }

    public void StopTimer()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
    }

    public void setConnectionIdUserReader(string connectionId)
    {
        ConnectionIdUserReadQRCode = connectionId;
    }

    public string UserReadQrCode()
    {
        return ConnectionIdUserReadQRCode;
    }

    private void StartTimer()
    {
        remainingTimeInSeconds = 60;
        _timer = new Timer(1000)// 1 segundo
        {
            Enabled = false
        };
        //a cada 1 segundo executa a funcao em Elapsed
        _timer.Elapsed += ElapsedTimer;
        _timer.Enabled = true;
    }

    private async void ElapsedTimer(object sender, ElapsedEventArgs e)
    {
        //decrementa o tempo
        if (remainingTimeInSeconds >= 0)
            //AddConnection é o hub que envia msg com caller
            await _hubContext
                    .Clients.Client(_qrCodeOwnerConnectionId)
                    .SendAsync("SetTempoRestante", remainingTimeInSeconds--);
        else
        {
            StopTimer();
            //executar a funcao para excluir o codigo do db
            //apos o tempo expirado
            _callbackExpiredTime(_qrCodeOwnerConnectionId);
        }
    }
}
