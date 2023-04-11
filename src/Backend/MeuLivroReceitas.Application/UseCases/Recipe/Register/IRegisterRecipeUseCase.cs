using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;

namespace MeuLivroReceitas.Application.UseCases.Recipe;

public interface IRegisterRecipeUseCase
{
    //RequestRegisterRecipeJson criado em comunication
    //ResponseRecipeJson criado em comunication
    Task<ResponseRecipeJson> Execute(RequestRegisterRecipeJson req);
}
