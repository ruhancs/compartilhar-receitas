using MeuLivroReceitas.Domain.Enum;

namespace MeuLivroReceitas.Domain.Entities;

public class Ingredient : EntityBase
{
    public string Product { get; set; }
    public string Quantity { get; set; }
    public long RecipeId { get; set; }
}
