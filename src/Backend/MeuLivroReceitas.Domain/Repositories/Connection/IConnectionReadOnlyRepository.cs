namespace MeuLivroReceitas.Domain.Repositories.Connection;

public interface IConnectionReadOnlyRepository
{
    Task<bool> ConnectionExist(long userIdA, long userIdB);
    Task<List<Entities.Usuario>> GetUserConnetions(long userId);
}
