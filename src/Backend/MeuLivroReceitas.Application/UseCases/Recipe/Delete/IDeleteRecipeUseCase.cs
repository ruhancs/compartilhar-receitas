namespace MeuLivroReceitas.Application.UseCases.Recipe.Delete
{
    public interface IDeleteRecipeUseCase
    {
        Task Execute(long recipeId);
    }
}
