using System.Text.Json;
using MetaCrudFosis.Api.Repositories;

namespace MetaCrudFosis.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // ILogRepository se resuelve por petición (Scoped/Singleton) inyectándolo en Invoke,
    // no en el constructor (el middleware es singleton y no debe capturar servicios scoped).
    public async Task InvokeAsync(HttpContext context, ILogRepository logs)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada en {Path}", context.Request.Path);

            // Registrar en LiteDB (NoSQL)
            logs.Add(
                level: "ERROR",
                message: ex.Message,
                source: context.Request.Path,
                details: ex.GetType().Name);

            // Respuesta 500 estandarizada, SIN stack trace
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