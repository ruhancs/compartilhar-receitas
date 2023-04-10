using MeuLivroReceitas.Comunication.Request;

namespace MeuLivroReceitas.Application.UseCases.User.Update;

//adicionar injecao de dependencia no bootstraped
public interface IUpdatePasswordUseCase
{
    Task Execute(RequestUpdatePasswordJson req);
}
