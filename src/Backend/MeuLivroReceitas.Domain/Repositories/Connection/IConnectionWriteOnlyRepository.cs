namespace MeuLivroReceitas.Domain.Repositories.Connection;

public interface IConnectionWriteOnlyRepository
{
    Task Register(Domain.Entities.Conexao connection);
    Task Remove(long userId, long userIdToDisconnect);
}
