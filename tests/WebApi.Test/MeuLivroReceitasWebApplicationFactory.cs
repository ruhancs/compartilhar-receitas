using MeuLivroReceitas.InfraStructure.AccessRpository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class MeuLivroReceitasWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //arquivo de contexto para teste dentro de appSettings.Json
        //criar um banco de dados em memoria
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                //se ja existir um Context remove
                var descript = services.SingleOrDefault(d => d.ServiceType == typeof(Context));
                if(descript != null)
                {
                    services.Remove(descript);
                }

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<Context>(opt =>
                {
                    opt.UseInMemoryDatabase("InMemoryDbForTesting");
                    opt.UseInternalServiceProvider(provider);
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedService = scope.ServiceProvider;

                var database = scopedService.GetRequiredService<Context>();

                //se ao iniciar o database ja estiver com dados ele deleta
                database.Database.EnsureDeleted();
            });
    }
}
