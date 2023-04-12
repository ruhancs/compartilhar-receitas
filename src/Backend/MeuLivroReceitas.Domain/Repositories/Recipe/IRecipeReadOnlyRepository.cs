namespace MeuLivroReceitas.Domain.Repositories.Recipe
{
    public interface IRecipeReadOnlyRepository
    {
        Task<List<Entities.Recipe>> GetAllRecipesUser(long userId);
    }
}
