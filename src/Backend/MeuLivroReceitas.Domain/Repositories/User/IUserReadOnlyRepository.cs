using MeuLivroReceitas.Domain.Entities;

namespace MeuLivroReceitas.Domain.Repositories.User;

//interface com as funçoes de leitura do usuario ex get
//Tasks implementadas em repository de infrastructure
public interface IUserReadOnlyRepository
{
    Task<bool> ExistUserEmail(string email);
    Task<Usuario> Login(string email, string password);
    Task<Usuario> GetEmail(string email);
}
