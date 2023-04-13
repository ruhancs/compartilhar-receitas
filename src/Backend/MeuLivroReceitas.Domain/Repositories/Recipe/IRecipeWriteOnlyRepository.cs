using MeuLivroReceitas.Domain.Entities;

namespace MeuLivroReceitas.Domain.Repositories.Recipe;

//Tasks implementadas em repository de infrastructure
//criar entidade em api entity
public interface IRecipeWriteOnlyRepository
{
    Task Register(Entities.Recipe recipe);

    Task Delete(long id);
}
