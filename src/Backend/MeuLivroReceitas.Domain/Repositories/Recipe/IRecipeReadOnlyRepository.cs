namespace MeuLivroReceitas.Domain.Repositories.Recipe
{
    public interface IRecipeReadOnlyRepository
    {
        Task<IList<Entities.Recipe>> GetAllRecipesUser(long userId);
        Task<Entities.Recipe> GetRecipesById(long recipeId);
        Task<int> QuantityRecipes(long userId);

        Task<IList<Entities.Recipe>> GetAllRecipesConnectedUsers(List<long> userIds);
    }
}
