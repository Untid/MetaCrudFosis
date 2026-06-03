using MetaCrudFosis.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MetaCrudFosis.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogController : ControllerBase
{
    private readonly ILogRepository _logs;

    public LogController(ILogRepository logs) => _logs = logs;

    [HttpGet]
    public IActionResult GetAll() => Ok(_logs.GetAll());

    // POST de prueba: /api/Log/test  → guarda un log de ejemplo
    [HttpPost("test")]
    public IActionResult AddTest()
    {
        var entry = _logs.Add("INFO", $"Log de prueba creado a las {DateTime.Now:HH:mm:ss}");
        return Ok(entry);
    }
}