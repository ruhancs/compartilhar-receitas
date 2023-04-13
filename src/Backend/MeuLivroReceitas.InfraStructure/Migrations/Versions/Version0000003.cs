using FluentMigrator;

namespace MeuLivroReceitas.InfraStructure.Migrations.Versions;

//EnumVersions.CreateUserTable indica a versao criada em EnumVersions
[Migration((long)EnumVersions.UpdateRecipeTable, "Adicionado coluna tempo de preparo")]
// Migration de fluentMigrator
public class Version0000003 : Migration
{
    public override void Down()
    {
        throw new NotImplementedException();
    }

    public override void Up()
    {
        //adicionar a coluna na tabela
        Alter.Table("receitas").AddColumn("PreparationTime")
            .AsInt32()
            .NotNullable()
            .WithDefaultValue(1);
    }
}
