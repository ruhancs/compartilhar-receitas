using FluentMigrator.Builders.Create.Table;

namespace MeuLivroReceitas.InfraStructure.Migrations;

public static class VersionBase
{
    public static ICreateTableColumnOptionOrWithColumnSyntax InsertCollumns(ICreateTableWithColumnOrSchemaOrDescriptionSyntax table)
    {
        return table
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("DataCriacao").AsDateTime().NotNullable();
        

    }
}
