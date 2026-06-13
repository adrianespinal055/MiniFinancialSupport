using Microsoft.EntityFrameworkCore;
using MiniFinancialSupport.Application.Interfaces;
using MiniFinancialSupport.Infrastructure.Persistence;
using MiniFinancialSupport.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using MiniFinancialSupport.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Controllers: nuestros endpoints REST viven en clases Controller (no en Minimal API).
builder.Services.AddControllers();

// FluentValidation — 2 piezas:
// 1) AutoValidation: ASP.NET corre el validador automáticamente y devuelve 400 si falla.
builder.Services.AddFluentValidationAutoValidation();
// 2) Registramos TODOS los validadores del assembly (busca a partir de CreateCustomerRequestValidator).
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerRequestValidator>();

// Swagger / OpenAPI: documentación interactiva + probador de la API en el navegador.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core: registramos el DbContext en Inyección de Dependencias usando el connection string.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registramos nuestro servicio: cuando alguien pida ICustomerService, DI entrega un CustomerService.
// Scoped = una instancia por cada request HTTP.
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Registramos el generador de tokens JWT.
builder.Services.AddScoped<IJwtService, JwtService>();

//Configuramos la AUTENTICACION por JWT: como validar los tokens que llegan
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"]!))


        };
    });

var app = builder.Build();

// Solo en desarrollo mostramos Swagger (no se expone en producción).
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();    // primero AUTENTICA: lee y valida el token (¿quién eres?)
app.UseAuthorization();     // luego AUTORIZA: revisa permisos/roles (¿qué puedes hacer?)

// Mapea las rutas a los métodos de los controllers.
app.MapControllers();

app.Run();
