using Microsoft.EntityFrameworkCore;
using MeuLivroReceitas.Domain.Entities;

namespace MeuLivroReceitas.InfraStructure.AccessRpository;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {}

    //para se conectar com a entidade Usuario
    //mapeamento da tabela User
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Recipe> Receitas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //aplicar todas as configuraçoes herdadas no construtor
        //faz as configuraçoes para fazer a conexao com a tabela Usuario
        //GET POST DELETE
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
    }
}
