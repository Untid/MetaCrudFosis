namespace MetaCrudFosis.Api.Models;

/// <summary>
/// Modelo de log para LiteDB (NoSQL). NO implementa IEntity a propósito:
/// así queda fuera del auto-registro de EF Core y del descubrimiento de
/// controladores genéricos. Vive únicamente en logs.db (LiteDB).
/// </summary>
public class SystemLog
{
    public int Id { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}