using MeuLivroReceitas.Comunication.Response;

namespace MeuLivroReceitas.Application.UseCases.User.GetPerfil;

public interface IGetPerfilUseCase
{
    Task<ResponsePerfilUserJson> Execute();
}
