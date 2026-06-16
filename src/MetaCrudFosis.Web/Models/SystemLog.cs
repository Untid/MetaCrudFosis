namespace MetaCrudFosis.Web.Models;

/// <summary>
/// Modelo de log en la capa Web. Es un espejo del SystemLog de la API, usado únicamente
/// para deserializar la respuesta JSON del endpoint de logs y mostrarla en la vista
/// "Actividad". No tiene lógica ni validación: es un objeto de transporte de solo lectura.
/// </summary>
public class SystemLog
{
    public int Id { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? Source { get; set; }
    public string? Details { get; set; }
}