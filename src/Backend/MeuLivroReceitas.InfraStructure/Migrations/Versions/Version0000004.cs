using FluentMigrator;

namespace MeuLivroReceitas.InfraStructure.Migrations.Versions;

//EnumVersions.CreateUserTable indica a versao criada em EnumVersions
[Migration((long)EnumVersions.UpdateRecipeTable, "Adicionado tabelas de associacao de usuarios")]
// Migration de fluentMigrator
public class Version0000004 : Migration
{
    public override void Down()
    {
        throw new NotImplementedException();
    }

    public override void Up()
    {
        var table = VersionBase.InsertCollumns(Create.Table("Codigos"));

        table
            .WithColumn("Code").AsString(2000).NotNullable()
            //ForeignKey da tabela receita com Usuario
            .WithColumn("UserId")
                .AsInt64()
                .NotNullable()
                .ForeignKey("FK_Codigo_Usuario_Id", "Usuarios", "Id");
    }
}
