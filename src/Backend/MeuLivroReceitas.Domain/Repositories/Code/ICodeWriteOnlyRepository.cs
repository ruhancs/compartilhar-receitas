namespace MeuLivroReceitas.Domain.Repositories.Code;

public interface ICodeWriteOnlyRepository
{
    Task Register(Domain.Entities.Codes code);
    Task Delete(long userId);
}
