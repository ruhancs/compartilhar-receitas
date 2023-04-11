using FluentMigrator;

namespace MeuLivroReceitas.InfraStructure.Migrations.Versions;

//EnumVersions.CreateUserTable indica a versao criada em EnumVersions
[Migration((long)EnumVersions.CreateUserTable, "Cria tabela usuario")]
public class Version0000001 : Migration
{
    public override void Down()
    {
    }

    // criar a tabela usuarios
    public override void Up()
    {
        var table = VersionBase.InsertCollumns(Create.Table("Usuarios"));

        table
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Email").AsString(200).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("Phone").AsString(14).NotNullable();


    }
}
