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

    //colecao de ingredients
    public ICollection<Ingredient> Ingredients { get; set; }
}
