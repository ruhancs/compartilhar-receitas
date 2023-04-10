using MeuLivroReceitas.Domain.Entities;

namespace MeuLivroReceitas.Application.Services.AuthUser;

public interface IAuthenticatedUser
{
    //GetUser implementado em Application AuthenticatedUser
    Task<Usuario> GetUser();
}
