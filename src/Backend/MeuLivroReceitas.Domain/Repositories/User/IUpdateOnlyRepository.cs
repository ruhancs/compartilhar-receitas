using MeuLivroReceitas.Domain.Entities;

namespace MeuLivroReceitas.Domain.Repositories.User;

public interface IUpdateOnlyRepository
{
    void Update(Usuario user);
}
