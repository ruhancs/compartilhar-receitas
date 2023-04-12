using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories.Recipe;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroReceitas.InfraStructure.AccessRpository.Repository;

//recebe as interface para implementar os metodos de leitura,escrita e update
//adicionar dependencia em bootstraped em AddRepositories
public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository
{
    //adicionar em Infrastructure em Context
    //public DbSet<Recipe> Receitas { get; set; }
    // para utilizar a Recipe no contexto
    private readonly Context _context;
    public RecipeRepository(Context context)
    {
        _context = context;
    }

    public async Task<IList<Recipe>> GetAllRecipesUser(long userId)
    {
        //adicionar Receitas.AsNoTracking() para melhorar performance
        return await _context.Receitas
            .Include(r => r.Ingredients)//incluir a tabela de ingredientes na consulta
            .AsNoTracking()
            .Where(r => r.UserId == userId).ToListAsync();
    }

    public async Task<Recipe> GetRecipesById(long recipeId)
    {
        return await _context.Receitas.AsNoTracking()
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == recipeId);
            
    }

    public async Task Register(Recipe recipe)
    {
        await _context.Receitas.AddAsync(recipe);
    }

}
