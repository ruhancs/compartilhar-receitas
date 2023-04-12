using MeuLivroReceitas.Comunication.Response;

namespace MeuLivroReceitas.Application.UseCases.Recipe.GetById;

public interface IGetRecipeByIdUseCase
{
    Task<ResponseRecipeJson> Execute(long recipeId);
}
