using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories.Recipe;

namespace MeuLivroReceitas.InfraStructure.AccessRpository.Repository;

//recebe as interface para implementar os metodos de leitura,escrita e update
//adicionar dependencia em bootstraped em AddRepositories
public class RecipeRepository : IRecipeWriteOnlyRepository
{
    //adicionar em Infrastructure em Context
    //public DbSet<Recipe> Receitas { get; set; }
    // para utilizar a Recipe no contexto
    private readonly Context _context;
    public RecipeRepository(Context context)
    {
        _context = context;
    }

    public async Task Register(Recipe recipe)
    {
        await _context.Receitas.AddAsync(recipe);
    }
}
