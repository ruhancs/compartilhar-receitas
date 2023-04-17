using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories.Recipe;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroReceitas.InfraStructure.AccessRpository.Repository;

//recebe as interface para implementar os metodos de leitura,escrita e update
//adicionar dependencia em bootstraped em AddRepositories
public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository
{
    //adicionar em Infrastructure em Context
    //public DbSet<Recipe> Receitas { get; set; }
    // para utilizar a Recipe no contexto
    private readonly Context _context;
    public RecipeRepository(Context context)
    {
        _context = context;
    }

    public async Task Delete(long RecipeId)
    {
        var recipe = await _context.Receitas
            .FirstOrDefaultAsync(r => r.Id == RecipeId);

        _context.Receitas.Remove(recipe);
    }

    public async Task<IList<Recipe>> GetAllRecipesUser(long userId)
    {
        //adicionar Receitas.AsNoTracking() para melhorar performance
        return await _context.Receitas
            .AsNoTracking()
            .Include(r => r.Ingredients)//incluir a tabela de ingredientes na consulta
            .Where(r => r.UserId == userId).ToListAsync();
    }
    
    public async Task<IList<Recipe>> GetAllRecipesConnectedUsers(List<long> userIds)
    {
        //retornar as receitas dos usuario que estao conectados
        //todos users que contem o userId
        return await _context.Receitas
            .AsNoTracking()
            .Include(r => r.Ingredients)//incluir a tabela de ingredientes na consulta
            .Where(r => userIds.Contains(r.UserId)).ToListAsync();
    }

    public async Task<Recipe> GetRecipesById(long recipeId)
    {
        return await _context.Receitas.AsNoTracking()
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == recipeId);
            
    }

    //pega receitas pelo id sem AsNoTracking para conseguir atualizar
    public async Task<Recipe> GetRecipesByIdForUpdate(long recipeId)
    {
        return await _context.Receitas
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == recipeId);
            
    }

    public async Task<int> QuantityRecipes(long userId)
    {
        return await _context.Receitas.CountAsync(r => r.UserId == userId);
    }

    public async Task Register(Recipe recipe)
    {
        await _context.Receitas.AddAsync(recipe);
    }

    public void Update(Recipe recipe)
    {
        _context.Receitas.Update(recipe);
    }
}
