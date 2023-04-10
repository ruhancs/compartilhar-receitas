using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;

namespace MeuLivroReceitas.Application.UseCases.User.Register;

//adicionar injecao de dependencia no bootstraped
public interface IRegisterUserUseCase
{
    Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson req);
}
