using MeuLivroReceitas.Comunication.Request;

namespace MeuLivroReceitas.Application.UseCases.Recipe.Update;

public interface IUpdateRecipeUseCase
{
    //usa a mesmo tipo de requisicao para registrar receita
    Task Execute(long recipeId, RequestRegisterRecipeJson req);
}
