using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;

namespace MeuLivroReceitas.Application.UseCases.Login.DoLogin;

public interface ILoginUseCase
{
    Task<ResponseLoginJson> Execute(RequestLoginJson req);
}
