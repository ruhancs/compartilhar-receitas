using MeuLivroReceitas.Comunication.Response;

namespace MeuLivroReceitas.Application.UseCases.Connection.Get;

public interface IGetAllConnectionsUseCase
{
    Task<IList<ResponseUserConnectedJson>> Execute();
}
