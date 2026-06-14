using System.Net;
using System.Text.Json;
using MiniFinancialSupport.Domain.Exceptions;

namespace MiniFinancialSupport.Api.Middleware;

// Middleware global de errores: envuelve TODA petición en un try/catch.
// Si algo lanza una excepción no controlada, la atrapamos AQUÍ (un solo lugar),
// la logueamos, y devolvemos un JSON limpio con 500 — sin filtrar el stack trace.
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;                       // el siguiente paso del pipeline
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    // RequestDelegate e ILogger llegan por inyección de dependencias.
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // ASP.NET llama InvokeAsync en cada petición.
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);   // deja pasar la petición al resto del pipeline (controllers, etc.)
        }
        catch (BusinessRuleException ex)
        {
            // Regla de negocio violada (saldo insuficiente, cuenta bloqueada...) -> 400, NO 500.
            _logger.LogWarning("Business rule violated: {Message}", ex.Message);

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                success = false,
                message = ex.Message   // el mensaje de la regla SÍ se muestra (es seguro y útil)
            }));
        }
        catch (Exception ex)
        {
            // Logueamos el error REAL (con stack trace) para nosotros, en el servidor.
            _logger.LogError(ex, "Unhandled exception for {Path}", context.Request.Path);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
            context.Response.ContentType = "application/json";

            // Respuesta LIMPIA para el cliente — NO exponemos el stack trace (secure coding).
            // El traceId permite correlacionar el error que vio el cliente con el log del servidor.
            var response = new
            {
                success = false,
                message = "An unexpected error occurred.",
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
