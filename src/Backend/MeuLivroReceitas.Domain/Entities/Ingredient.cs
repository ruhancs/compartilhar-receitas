using System.ComponentModel.DataAnnotations.Schema;
using MeuLivroReceitas.Domain.Enum;

namespace MeuLivroReceitas.Domain.Entities;

// para estar igual a tabela criada em version0000002 em infrastructure
[Table("Ingredientes")]
public class Ingredient : EntityBase
{
    public string Product { get; set; }
    public string Quantity { get; set; }
    public long RecipeId { get; set; }
}
