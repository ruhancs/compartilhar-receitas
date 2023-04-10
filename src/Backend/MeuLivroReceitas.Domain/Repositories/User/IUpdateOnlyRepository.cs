using MeuLivroReceitas.Domain.Entities;

namespace MeuLivroReceitas.Domain.Repositories.User;

public interface IUpdateOnlyRepository
{
    //classes implementadas em infrastructure UserRepository
    void Update(Usuario user);
    Task<Usuario> GetById(long id);//pegar o usuario pelo id para atualizalo 
}
