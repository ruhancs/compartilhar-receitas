using System.Reflection;
using FluentMigrator.Runner;
using MeuLivroReceitas.Domain.Extension;
using MeuLivroReceitas.Domain.Repositories;
using MeuLivroReceitas.InfraStructure.AccessRpository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroReceitas.InfraStructure;

//Classe para replicar builder.Service
//extension de builder.Service
//utilizada para injetar dependencias
public static class Bootstrappers
{
    public static void AddRepository(this IServiceCollection service, IConfiguration configManager)
    {
        AddFluentMigrator(service, configManager);

        AddRepositories(service);
        AddUnitOfWork(service);
        AddContext(service, configManager);
    }

    //injeçao de dependencia de Context
    private static void AddContext(IServiceCollection services,IConfiguration configManager)
    {
        bool.TryParse(configManager.GetSection("Config:DataBaseInMemory").Value, out bool databaseInMemory);

        if (!databaseInMemory)
        {
            //versao do mysql
            var serverVersion = new MySqlServerVersion(new Version(8,0,32));
            var connectionStrig = configManager.GetFullConnectionString();

            //se em algum lugar for utilizado Context
            //ira entregar uma instancia da classe Context
            //que faz as operaçoes nas tabelas
            services.AddDbContext<Context>(dbContextOpt =>
            {
                dbContextOpt.UseMySql(connectionStrig, serverVersion);
            });
        }

    }

    //adicionar serviço UnitOfWork
    private static void AddUnitOfWork(IServiceCollection services)
    {
        //se em algum lugar for utilizado IUnitOfWork
        //ira entregar uma instancia da classe UnitOfWork
        //que faz o commit das alteraçoes
        //e libera a memoria
        services.AddScoped<IUnitOfWork, UnitOfWork>();    
    }

    private static void AddRepositories(IServiceCollection services)
    {
        //se em algum lugar for utilizado IUserWriteOnlyRepository e IUserReadOnlyRepository
        //ira entregar uma instancia da classe UserRepository
        //que adiciona usuarios ao db e verifica se email existe

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>()
            .AddScoped<IUserReadOnlyRepository, UserRepository>();
    }

    private static void AddFluentMigrator(IServiceCollection service, IConfiguration configManager)
    {
        var connectionString = configManager.GetFullConnectionString();

        //quando executar os testes verificar se esta usando um db em memory
        bool.TryParse(configManager.GetSection("Config:DataBaseInMemory").Value, out bool databaseInMemory);

        //se o db nao for em memoria 
        if (!databaseInMemory)
        {
            service.AddFluentMigratorCore().ConfigureRunner(c =>
            c.AddMySql5()
            .WithGlobalConnectionString(connectionString).ScanIn(Assembly.Load("MeuLivroReceitas.InfraStructure"))
            .For.All()//procurar em todo MeuLivroReceitas.InfraStructure classes para migrar
            );
        }
    }
}