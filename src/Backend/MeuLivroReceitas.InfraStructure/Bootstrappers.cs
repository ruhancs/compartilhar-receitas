using System.Reflection;
using FluentMigrator.Runner;
using MeuLivroReceitas.Domain.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroReceitas.InfraStructure;

//Classe para replicar builder.Service
//extension de builder.Service
//utilizada para injetar dependencias
public static class Bootstrappers
{
    public static void AddRepository(this IServiceCollection service, IConfiguration configManager)
    {
        AddFluentMigrator(service, configManager);
    }

    private static void AddFluentMigrator(IServiceCollection service, IConfiguration configManager)
    {
        var connectionString = configManager.GetFullConnectionString();
        
        service.AddFluentMigratorCore().ConfigureRunner(c => 
        c.AddMySql5()
        .WithGlobalConnectionString(connectionString).ScanIn(Assembly.Load("MeuLivroReceitas.InfraStructure"))
        .For.All()//procurar em todo MeuLivroReceitas.InfraStructure classes para migrar
        );
    }
}
