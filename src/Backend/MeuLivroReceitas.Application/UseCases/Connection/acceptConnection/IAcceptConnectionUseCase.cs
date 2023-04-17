namespace MeuLivroReceitas.Application.UseCases.Connection.acceptConnection;

public interface IAcceptConnectionUseCase
{
    Task<string> Execute(string userToConnect);
}
