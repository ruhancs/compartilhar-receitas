using MeuLivroReceitas.InfraStructure.Migrations;
using MeuLivroReceitas.Domain.Extension;
using MeuLivroReceitas.InfraStructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//inje�ao de dependencia
//para utilizar IMigrationRunner em MigrationExtention
builder.Services.AddRepository(builder.Configuration);

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
    //GetConnection criado em Domain/Extention
    var conectionString = builder.Configuration.GetConnection();
    var databaseName = builder.Configuration.GetDatabaseName();

    Database.CreateDatabase(conectionString, databaseName);

    //fazer as migracoes do db
    app.MigrationDB();
}
