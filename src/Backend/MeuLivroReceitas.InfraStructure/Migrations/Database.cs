using Dapper;
using MySqlConnector;

namespace MeuLivroReceitas.InfraStructure.Migrations;

public static class Database
{
    public static void CreateDatabase(string stringConnection, string databaseName)
    {
        //using quando a funcao terminar fecha a conexao e libera a memoria
        using var myConnection = new MySqlConnection(stringConnection);

        var parameters = new DynamicParameters();
        //indica que nome sera o databaseName na query em myConnection.Query
        parameters.Add("nome", databaseName);

        //verificar se existe o database
        //FROM INFORMATION_SCHEMA.SCHEMATA tabela interna do sql
        var registers = myConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @nome",parameters);

        if (!registers.Any())
        {
            //se o database nao existe cria ele
            myConnection.Execute($"CREATE DATABASE {databaseName}");
        } 
    }
}
