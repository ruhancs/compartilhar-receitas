namespace MeuLivroReceitas.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}

