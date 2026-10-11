using DPA.SERVICIOTECNICO.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Cadena de conexión DevConnection, definida en appsettings.json.
var cnx = builder.Configuration.GetConnectionString("DevConnection");
builder.Services.AddDbContext<ServicioTecnicoDbContext>(options => options.UseSqlServer(cnx));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
