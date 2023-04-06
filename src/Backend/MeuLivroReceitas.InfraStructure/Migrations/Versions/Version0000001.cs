using FluentMigrator;

namespace MeuLivroReceitas.InfraStructure.Migrations.Versions;

//EnumVersions.CreateUserTable indica a versao criada em EnumVersions
[Migration((long)EnumVersions.CreateUserTable, "Cria tabela usuario")]
public class Version0000001 : Migration
{
    public override void Down()
    {
    }

    public override void Up()
    {
        var table = VersionBase.InsertCollumns(Create.Table("Usuario"));

        table
            .WithColumn("Nome").AsString(100).NotNullable()
            .WithColumn("Email").AsString(200).NotNullable()
            .WithColumn("Senha").AsString(2000).NotNullable()
            .WithColumn("Telefone").AsString(14).NotNullable();
            

    }
}
