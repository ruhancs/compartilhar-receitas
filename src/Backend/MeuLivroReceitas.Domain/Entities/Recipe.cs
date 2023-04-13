using MeuLivroReceitas.Domain.Enum;

namespace MeuLivroReceitas.Domain.Entities;

//EntityBase contem Id CreatedAt
public class Recipe : EntityBase
{
    public string Title { get; set; }

    //Category é o enum com as opcoes de categoria
    // criado em domain Enum
    public Category Category { get; set; }
    public string MethodPreparation { get; set; }
    public int PreparationTime { get; set; }

    //colecao de ingredients
    //na requisiçao precisa informar que deseja inserir os ingrdientes
    //join com a tabela de ingredientes
    //em infrastructure RecipeRepository
    public ICollection<Ingredient> Ingredients { get; set; }
    public long UserId { get; set; }
}
