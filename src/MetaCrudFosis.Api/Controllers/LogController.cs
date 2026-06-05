using MetaCrudFosis.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MetaCrudFosis.Api.Controllers;

// DTO de entrada para registrar logs desde fuera (p.ej. el generador T4 en Fase 3)
public record LogEntradaDto(string Level, string Message, DateTime? Timestamp);

[ApiController]
[Route("api/[controller]")]
public class LogController : ControllerBase
{
    private readonly ILogRepository _logs;

    public LogController(ILogRepository logs) => _logs = logs;

    [HttpGet]
    public IActionResult GetAll() => Ok(_logs.GetAll());

    [HttpPost("test")]
    public IActionResult AddTest()
    {
        var entry = _logs.Add("INFO", $"Log de prueba creado a las {DateTime.Now:HH:mm:ss}");
        return Ok(entry);
    }

    // POST /api/Log  → registra un log desde un servicio externo.
    // El Timestamp recibido se ignora a propósito: la hora la marca el servidor
    // (coherencia y seguridad). El generador T4 mandará Level="SUCCESS" y el mensaje.
    [HttpPost]
    public IActionResult Add([FromBody] LogEntradaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Level) || string.IsNullOrWhiteSpace(dto.Message))
            return BadRequest("Level y Message son obligatorios.");

        var entry = _logs.Add(dto.Level, dto.Message);
        return Ok(entry);
    }
}