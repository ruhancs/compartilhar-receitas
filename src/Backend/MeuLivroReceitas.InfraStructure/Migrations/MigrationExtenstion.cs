using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroReceitas.InfraStructure.Migrations;

public static class MigrationExtension
{
    //utilizada com app em program para fazer as migraçoes
    //IApplicationBuilder para o app poder utilizar a funçao
    public static void MigrationDB(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();//lista todas migraçoes disponiveis

        runner.MigrateUp();//chama todas as funçoes up das migraçoes
    }
}
