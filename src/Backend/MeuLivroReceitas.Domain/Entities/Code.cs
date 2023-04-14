namespace MeuLivroReceitas.Domain.Entities;

public class Codes : EntityBase
{
    public string Code { get; set; }
    public long UserId { get; set; }
}
