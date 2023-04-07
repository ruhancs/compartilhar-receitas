using Microsoft.Extensions.Configuration;

namespace MeuLivroReceitas.Domain.Extension;

public static class RepositoryExtension
{
    //IConfiguration para receber builder.Configuration
    public static string GetConnection(this IConfiguration configManager)
    {
        var conectionString = configManager.GetConnectionString("Connection");

        return conectionString;
    }
    public static string GetDatabaseName(this IConfiguration configManager)
    {
        var databaseName = configManager.GetConnectionString("DatabaseName");

        return databaseName;
    }

    public static string GetFullConnectionString(this IConfiguration configManager)
    {
        var dbName = configManager.GetDatabaseName();
        var connection = configManager.GetConnection();

        return $"{connection};Database={dbName}";
    }
}
