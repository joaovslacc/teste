using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.Repositories;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddTransient<IMovimentoRepository, MovimentoRepository>();
builder.Services.AddTransient<IContaCorrenteRepository, ContaCorrenteRepository>();

// Configura MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Configura��o do SQLite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Configura Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDbConnection>(provider =>
{
    var config = provider.GetRequiredService<DatabaseConfig>();
    return new SqliteConnection(config.Name); // Aqui voc� cria uma nova conex�o SQLite
});




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

// Configura��o do SQLite
app.Services.GetRequiredService<IDatabaseBootstrap>().Setup();



app.Run();
