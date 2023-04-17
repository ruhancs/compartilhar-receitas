using System.ComponentModel.DataAnnotations.Schema;

namespace MeuLivroReceitas.Domain.Entities;

[Table("Conexoes")]
public class Conexao : EntityBase
{
    public long UserId { get; set; }
    public long UserConnectedId { get; set; }

    //para utilizar pegar a entidade Usuario
    // de UserIdConnected Join com a tabela Usuario
    //o nome deve ser igual a UserConnectedId que contem o id
    //so que sem o id ao final
    public Usuario UserConnected { get; set; }

}
