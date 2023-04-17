using HashidsNet;
using MeuLivroReceitas.Api.Filters;
using MeuLivroReceitas.Api.Filters.AuthenticatedUser;
using MeuLivroReceitas.Api.Filters.Swagger;
using MeuLivroReceitas.Api.Middleware;
using MeuLivroReceitas.Api.WebSockets;
using MeuLivroReceitas.Application;
using MeuLivroReceitas.Application.Services.Automapper;
using MeuLivroReceitas.Domain.Extension;
using MeuLivroReceitas.InfraStructure;
using MeuLivroReceitas.InfraStructure.AccessRpository;
using MeuLivroReceitas.InfraStructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

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
    //para descriptografar o id em Api.Filters.Swagger HashIdOperationFilter
    c.OperationFilter<HashIdOperationFilter>();

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
          Array.Empty<string>()
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

//AutomapperConfig criado em Application.Services.Automapper
//habilitar o automapper sem hashId
//builder.Services.AddAutoMapper(typeof(AutomapperConfig));

//habilitar automapper com as configuraçoes que estao em AutomepperConfig
builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(
    cfg =>
    {
        cfg.AddProfile(new AutomapperConfig(provider.GetService<IHashids>()));
    }).CreateMapper());

//para utilizar a Policy de authenticacao de usuario em Api.WebSockets addConnection
builder.Services.AddScoped<IAuthorizationHandler, LoggedUserHandler>();

//configurar para quando utilizar a Policy LoggedUser em Api.WebSockets
//utilizar Filters.AuthenticatedUser LoggedUserHandler
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("LoggedUser", policy => policy.Requirements.Add(new LoggedUserRequirements()));
});

//filtro de authenticacao de usuario
builder.Services.AddScoped<AuthenticatedUserAttr>();

//para utilizar o Websocket config do SignalR
//com o app construido mappear o hub
builder.Services.AddSignalR();

//monitoracao da api health check
//endpoint para monitorar o estado da api
builder.Services.AddHealthChecks().AddDbContextCheck<Context>();

var app = builder.Build();

//rota para acessar health check
app.MapHealthChecks("/health", new HealthCheckOptions
{
    AllowCachingResponses = false,//sem cache do estado
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

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

//mappear o Hub criado em Api.WebSockets AddConnection
// /addConexao e a rota(url) para conectar no hub
app.MapHub<AddConnection>("/addConexao");

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