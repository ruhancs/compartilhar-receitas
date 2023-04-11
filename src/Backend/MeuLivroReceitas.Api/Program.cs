using MeuLivroReceitas.InfraStructure.Migrations;
using MeuLivroReceitas.Domain.Extension;
using MeuLivroReceitas.InfraStructure;
using MeuLivroReceitas.Api.Filters;
using MeuLivroReceitas.Application.Services.Automapper;
using MeuLivroReceitas.Application;
using MeuLivroReceitas.InfraStructure.AccessRpository;
using MeuLivroReceitas.Application.Services.AuthUser;
using MeuLivroReceitas.Application.Services.AuthenticatedUser;
using Microsoft.OpenApi.Models;
using MeuLivroReceitas.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//endpoints com letra minuscula
builder.Services.AddRouting(option => option.LowercaseUrls = true);

//para utilizar o IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//adicionando autenticacao por token no swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MeuLivroReceitas", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Header de autoriza��o JWT usando o esquema Bearer.\r\n\r\nInforme 'Bearer'[espa�o] e o seu token.\r\n\r\nExamplo: \'Bearer 12345abcdef\'",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
          new OpenApiSecurityScheme
          {
             Reference = new OpenApiReference
             {
                 Type = ReferenceType.SecurityScheme,
                 Id = "Bearer"
             }
          },
          new string[] {}
       }
    });

});

//inje�ao de dependencia
//para utilizar IMigrationRunner em MigrationExtention
builder.Services.AddInfrastructure(builder.Configuration);

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

//filtro de authenticacao de usuario
builder.Services.AddScoped<AuthenticatedUserAttr>();

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

//para adicionar a opcao de escolher a linguagem
app.UseMiddleware<CultureMiddleware>();

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

//retirar alerta de bugs do sonar
#pragma warning disable CA1058, S3903, S1118
//definicao para Utilizar Program nos testes
public partial class Program { }
#pragma warning disable CA1058, S3903, S1118