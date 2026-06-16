using System.Text.Json;
using MetaCrudFosis.Api.Repositories;

namespace MetaCrudFosis.Api.Middlewares;

/// <summary>
/// Middleware global de manejo de excepciones. Envuelve toda la tubería de peticiones:
/// si cualquier punto de la API lanza una excepción no controlada, se captura aquí,
/// se registra como ERROR en la auditoría (LiteDB) y se devuelve una respuesta 500
/// estandarizada SIN exponer el stack trace, evitando filtrar información interna.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // El ILogRepository se inyecta aquí, en Invoke, y NO en el constructor:
    // el middleware se instancia una sola vez (singleton) y no debe capturar
    // servicios de vida más corta (scoped); recibirlos por parámetro lo resuelve.
    public async Task InvokeAsync(HttpContext context, ILogRepository logs)
    {
        try
        {
            await _next(context);   // continúa la cadena normal de la petición
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada en {Path}", context.Request.Path);

            // Se deja constancia del error en la auditoría documental (LiteDB).
            logs.Add(
                level: "ERROR",
                message: ex.Message,
                source: context.Request.Path,
                details: ex.GetType().Name);

            // Respuesta 500 controlada: mensaje genérico, nunca el detalle interno.
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var payload = JsonSerializer.Serialize(new
            {
                error = "Se produjo un error interno en el servidor.",
                status = 500
            });
            await context.Response.WriteAsync(payload);
        }
    }
}