using DPA.SERVICIOTECNICO.CORE.Core.Interfaces;
using DPA.SERVICIOTECNICO.CORE.Infrastructure.Data;
using DPA.SERVICIOTECNICO.CORE.Infrastructure.Repositories;
using DPA.SERVICIOTECNICO.CORE.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Cadena de conexión DevConnection, definida en appsettings.json.
var cnx = builder.Configuration.GetConnectionString("DevConnection");
builder.Services.AddDbContext<ServicioTecnicoDbContext>(options => options.UseSqlServer(cnx));

// Repositorio y servicio de órdenes de reparación, con el mismo ciclo de vida que el DbContext (una instancia
// por solicitud).
builder.Services.AddScoped<IOrdenReparacionRepository, OrdenReparacionRepository>();
builder.Services.AddScoped<IOrdenReparacionService, OrdenReparacionService>();

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
