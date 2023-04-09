using MeuLivroReceitas.InfraStructure.Migrations;
using MeuLivroReceitas.Domain.Extension;
using MeuLivroReceitas.InfraStructure;
using MeuLivroReceitas.Api.Filters;
using MeuLivroReceitas.Application.Services.Automapper;
using MeuLivroReceitas.Application;
using MeuLivroReceitas.InfraStructure.AccessRpository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//endpoints com letra minuscula
builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//inje�ao de dependencia
//para utilizar IMigrationRunner em MigrationExtention
builder.Services.AddRepository(builder.Configuration);

//injeçao de dependencia
//quando IRegisterUserUseCase for chamado instacia RegisterUserUseCase
builder.Services.AddApplication(builder.Configuration);

//pegar erros
builder.Services.AddMvc(option => option.Filters.Add(typeof(ExceptionsFilter)));

//habilitar automapper com as configuraçoes que estao em AutomepperConfig
builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(
    cfg =>
    {
        cfg.AddProfile(new AutomapperConfig());
    }).CreateMapper());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

updateDataBase();

app.Run();

void updateDataBase()
{
    //se o db for em memoria dos testes nao executa as migraçoes
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    
    using var context = serviceScope.ServiceProvider.GetService<Context>();

    //verificar se o provider name e InMemory
    bool? databaseInMemory = context?.Database?.ProviderName?.Equals("Microsoft.EntityFrameworkCore.InMemory");

    if(!databaseInMemory.HasValue || !databaseInMemory.Value)
    {
        //GetConnection criado em Domain/Extention
        var conectionString = builder.Configuration.GetConnection();
        var databaseName = builder.Configuration.GetDatabaseName();

        Database.CreateDatabase(conectionString, databaseName);

        //fazer as migracoes do db
        app.MigrationDB();
    }

}

//definicao para Utilizar Program nos testes
public partial class Program { }