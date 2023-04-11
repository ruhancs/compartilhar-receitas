using FluentMigrator;

namespace MeuLivroReceitas.InfraStructure.Migrations.Versions;

//EnumVersions.CreateUserTable indica a versao criada em EnumVersions
[Migration((long)EnumVersions.CreateRecipeTable, "Cria tabela receitas")]
// Migration de fluentMigrator
public class Version0000002 : Migration
{
    public override void Down()
    {
        
    }

    public override void Up()
    {
        CreateRecipeTable();
        CreateIngredientsTable();
    }

    private void CreateRecipeTable()
    {
        var table = VersionBase.InsertCollumns(Create.Table("Receitas"));

        table
            .WithColumn("Title").AsString(100).NotNullable()
            .WithColumn("Category").AsInt16().NotNullable()
            .WithColumn("MethodPreparation").AsString(5000).NotNullable();

    }
    private void CreateIngredientsTable()
    {
        var table = VersionBase.InsertCollumns(Create.Table("Ingredientes"));

        table
            .WithColumn("Product").AsString(100).NotNullable()
            .WithColumn("Quantity").AsString(100).NotNullable()
            //ForeignKey da tabela ingredientes com receita
            .WithColumn("RecipeId")
                .AsInt64()
                .NotNullable()
                //primeiro parametro nome da chave,
                //segundo o nome da tabela para associar,
                //terceiro parametro o nome da coluna em receitas que se associara a ingredientes
                .ForeignKey("FK_Ingrediente_Recipe_Id", "Receitas", "Id");

    }
}
