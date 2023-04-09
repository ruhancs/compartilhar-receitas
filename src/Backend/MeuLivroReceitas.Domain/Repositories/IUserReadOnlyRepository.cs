using MeuLivroReceitas.Domain.Entities;

namespace MeuLivroReceitas.Domain.Repositories;

//interface com as funçoes de leitura do usuario ex get
public interface IUserReadOnlyRepository
{
    Task<bool> ExistUserEmail(string email);
    Task<Usuario> Login(string email, string password); 
}
