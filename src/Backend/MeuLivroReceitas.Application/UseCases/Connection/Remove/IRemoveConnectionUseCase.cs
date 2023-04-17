namespace MeuLivroReceitas.Application.UseCases.Connection.Remove;

public interface IRemoveConnectionUseCase
{
    Task Execute(long userConnectedId);
}
