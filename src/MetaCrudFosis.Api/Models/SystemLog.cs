namespace MetaCrudFosis.Api.Models;

/// <summary>
/// Modelo de log para LiteDB (NoSQL). NO implementa IEntity a propósito:
/// así queda EXCLUIDO del auto-registro de EF Core y del descubrimiento de
/// controladores genéricos. De este modo los logs no generan tabla en SQLite
/// ni endpoint CRUD genérico; viven únicamente en logs.db (LiteDB), separando
/// físicamente los datos de negocio (SQLite) de la auditoría (LiteDB).
/// </summary>
public class SystemLog
{
    public int Id { get; set; }
    public string Level { get; set; } = string.Empty;     // INFO, ERROR, SUCCESS...
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }               // la marca siempre el servidor
    public string? Source { get; set; }                   // origen (p. ej. la ruta que falló)
    public string? Details { get; set; }                  // detalle extra (p. ej. tipo de excepción)
}