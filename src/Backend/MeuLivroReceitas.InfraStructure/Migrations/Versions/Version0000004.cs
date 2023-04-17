using FluentMigrator;

namespace MeuLivroReceitas.InfraStructure.Migrations.Versions;

//EnumVersions.CreateUserTable indica a versao criada em EnumVersions
[Migration((long)EnumVersions.CreateTableConnectUser, "Adicionado tabelas de associacao de usuarios")]
// Migration de fluentMigrator
public class Version0000004 : Migration
{
    public override void Down()
    {
        throw new NotImplementedException();
    }

    public override void Up()
    {
        CreateCodeTable();

        CreateConnectionTable();
    }

    private void CreateCodeTable()
    {
        var table = VersionBase.InsertCollumns(Create.Table("Codigos"));

        table
            .WithColumn("Code").AsString(2000).NotNullable()
            //ForeignKey da tabela Codigo com Usuario
            .WithColumn("UserId")
                .AsInt64()
                .NotNullable()
                .ForeignKey("FK_Codigo_Usuario_Id", "Usuarios", "Id");
    }
    
    private void CreateConnectionTable()
    {
        var table = VersionBase.InsertCollumns(Create.Table("Conexoes"));

        table
            .WithColumn("UserId")
                .AsInt64()
                .NotNullable()
                .ForeignKey("FK_Conexao_Usuario_Id", "Usuarios", "Id")
            .WithColumn("UserConnectedId")
                .AsInt64()
                .NotNullable()
                .ForeignKey("FK_Conexao_UserIdConnected", "Usuarios", "Id");
    }
}