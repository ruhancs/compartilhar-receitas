namespace MeuLivroReceitas.Domain.Repositories.Recipe
{
    public interface IRecipeReadOnlyRepository
    {
        Task<IList<Entities.Recipe>> GetAllRecipesUser(long userId);
    }
}
