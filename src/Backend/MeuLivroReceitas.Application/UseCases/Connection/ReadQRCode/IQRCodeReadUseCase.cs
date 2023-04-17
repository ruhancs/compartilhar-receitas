using MeuLivroReceitas.Comunication.Response;

namespace MeuLivroReceitas.Application.UseCases.Connection.ReadQRCode;

public interface IQRCodeReadUseCase
{
    Task<(ResponseConnectedUserJson userToConnect, string ownerQRCodeId)> Execute(string connectionCode);
}
