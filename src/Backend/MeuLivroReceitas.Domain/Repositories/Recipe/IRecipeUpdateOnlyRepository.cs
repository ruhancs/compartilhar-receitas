namespace MeuLivroReceitas.Domain.Repositories.Recipe;

public interface IRecipeUpdateOnlyRepository
{
    void Update(Entities.Recipe recipe);
    Task<Entities.Recipe> GetRecipesByIdForUpdate(long recipeId);
}
