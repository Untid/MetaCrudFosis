using MetaCrudFosis.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MetaCrudFosis.Api.Controllers;

/// <summary>
/// DTO de entrada para registrar logs desde un servicio externo a la API.
/// Lo usa, por ejemplo, el Generador (Fase 3) para dejar constancia de cada
/// entidad generada. Se define como record por ser un objeto inmutable de transferencia.
/// </summary>
public record LogEntradaDto(string Level, string Message, DateTime? Timestamp);

/// <summary>
/// Controlador de auditoría. Permite consultar los logs y registrarlos
/// (tanto internamente como desde servicios externos). Los logs se persisten
/// en LiteDB (NoSQL), separados de los datos de negocio que viven en SQLite.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LogController : ControllerBase
{
    private readonly ILogRepository _logs;

    public LogController(ILogRepository logs) => _logs = logs;

    // Devuelve todos los logs registrados (usado por la vista "Actividad" de la Web).
    [HttpGet]
    public IActionResult GetAll() => Ok(_logs.GetAll());

    // Endpoint de prueba para verificar manualmente que el logging funciona.
    [HttpPost("test")]
    public IActionResult AddTest()
    {
        var entry = _logs.Add("INFO", $"Log de prueba creado a las {DateTime.Now:HH:mm:ss}");
        return Ok(entry);
    }

    // POST /api/Log → registra un log enviado desde un servicio externo (p. ej. el Generador).
    // DECISIÓN DE SEGURIDAD/COHERENCIA: el Timestamp recibido en el DTO se IGNORA a propósito;
    // la marca de tiempo la pone siempre el servidor, evitando que un cliente falsee la hora.
    [HttpPost]
    public IActionResult Add([FromBody] LogEntradaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Level) || string.IsNullOrWhiteSpace(dto.Message))
            return BadRequest("Level y Message son obligatorios.");

        var entry = _logs.Add(dto.Level, dto.Message);
        return Ok(entry);
    }
}