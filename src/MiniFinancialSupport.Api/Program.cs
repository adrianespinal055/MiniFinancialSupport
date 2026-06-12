using Microsoft.EntityFrameworkCore;
using MiniFinancialSupport.Application.Interfaces;
using MiniFinancialSupport.Infrastructure.Persistence;
using MiniFinancialSupport.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers: nuestros endpoints REST viven en clases Controller (no en Minimal API).
builder.Services.AddControllers();

// Swagger / OpenAPI: documentación interactiva + probador de la API en el navegador.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core: registramos el DbContext en Inyección de Dependencias usando el connection string.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registramos nuestro servicio: cuando alguien pida ICustomerService, DI entrega un CustomerService.
// Scoped = una instancia por cada request HTTP.
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

// Solo en desarrollo mostramos Swagger (no se expone en producción).
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Mapea las rutas a los métodos de los controllers.
app.MapControllers();

app.Run();
