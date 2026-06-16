using MetaCrudFosis.Api.Models;
using MetaCrudFosis.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace MetaCrudFosis.Api.Controllers;

/// <summary>
/// Controlador genérico que resuelve el CRUD de CUALQUIER entidad que implemente IEntity.
/// Es la pieza central del proyecto: en lugar de escribir un controlador por entidad
/// (ProductoController, ZapatoController...), un único controlador genérico sirve a todas.
/// Las plantillas T4 NO generan este archivo; es fijo y se reutiliza para cada entidad
/// gracias a Reflection y a la convención de rutas configurada en Program.cs.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GenericController<T> : ControllerBase where T : class, IEntity
{
    // El repositorio genérico (acceso a datos) y el repositorio de logs se reciben
    // por inyección de dependencias; el contenedor de ASP.NET los proporciona.
    private readonly IGenericRepository<T> _repository;
    private readonly ILogRepository _logs;

    public GenericController(IGenericRepository<T> repository, ILogRepository logs)
    {
        _repository = repository;
        _logs = logs;
    }

    // GET /api/Producto                        → todos
    // GET /api/Producto?nombre=Teclado         → filtra por nombre
    // GET /api/Producto?stock=15&precio=79.99  → varios filtros combinados (AND)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        // Se leen los parámetros de la query string como pares campo=valor.
        // Si no hay ninguno, se devuelven todos; si los hay, se delega el filtrado
        // dinámico al repositorio (que construye la consulta por árboles de expresión).
        var filters = Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
        return filters.Count == 0
            ? Ok(await _repository.GetAllAsync())
            : Ok(await _repository.FindAsync(filters));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<T>> GetById(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? NotFound() : Ok(entity);   // 404 si no existe
    }

    [HttpPost]
    public async Task<ActionResult<T>> Create([FromBody] T entity)
    {
        var created = await _repository.AddAsync(entity);
        // Auditoría: se registra el alta. TryLog garantiza que un fallo de log
        // nunca interrumpa la operación de negocio (ver método TryLog más abajo).
        TryLog("INFO", $"Entidad {typeof(T).Name} creada (Id={created.Id}).");
        return Created($"/api/{typeof(T).Name}/{created.Id}", created);   // 201 + Location
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] T entity)
    {
        // Coherencia: el Id de la URL debe coincidir con el del cuerpo enviado.
        if (id != entity.Id)
            return BadRequest("El Id de la ruta no coincide con el del cuerpo.");

        var ok = await _repository.UpdateAsync(entity);
        return ok ? NoContent() : NotFound();   // 204 si actualiza, 404 si no existe
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _repository.DeleteAsync(id);
        if (!ok) return NotFound();

        TryLog("INFO", $"Entidad {typeof(T).Name} eliminada (Id={id}).");
        return NoContent();   // 204: borrado correcto sin contenido que devolver
    }

    // Exporta todos los registros como archivo JSON descargable.
    [HttpGet("export/json")]
    public async Task<IActionResult> ExportJson()
    {
        var data = await _repository.GetAllAsync();
        var json = JsonSerializer.SerializeToUtf8Bytes(data,
            new JsonSerializerOptions { WriteIndented = true });   // formato legible

        return File(json, "application/json", $"{typeof(T).Name}.json");
    }

    // Exporta todos los registros como archivo XML descargable.
    [HttpGet("export/xml")]
    public async Task<IActionResult> ExportXml()
    {
        var data = (await _repository.GetAllAsync()).ToList();

        // XmlSerializer necesita un tipo concreto; List<T> se resuelve en tiempo de ejecución.
        var serializer = new XmlSerializer(typeof(List<T>));
        using var stream = new MemoryStream();
        serializer.Serialize(stream, data);

        return File(stream.ToArray(), "application/xml", $"{typeof(T).Name}.xml");
    }

    // Inserción masiva: recibe una lista y la inserta de una vez.
    // Usado por la importación "Bulk Paste" (pegado desde Excel/JSON) de la Web.
    [HttpPost("bulk")]
    public async Task<IActionResult> CreateBulk([FromBody] List<T> entities)
    {
        if (entities is null || entities.Count == 0)
            return BadRequest("No se recibió ningún elemento.");

        var insertados = 0;
        foreach (var entity in entities)
        {
            await _repository.AddAsync(entity);
            insertados++;
        }

        _logs.Add("INFO", $"Importación masiva: {insertados} entidades {typeof(T).Name} creadas.");
        return Ok(new { insertados });   // devuelve cuántos se insertaron
    }

    /// <summary>
    /// Registro de log "a prueba de fallos": el logging es un servicio secundario
    /// (auditoría) y nunca debe tumbar la operación principal. Si LiteDB falla,
    /// se ignora el error y el CRUD sigue respondiendo correctamente.
    /// </summary>
    private void TryLog(string level, string message)
    {
        try { _logs.Add(level, message); }
        catch { /* no propagar: un fallo de log no debe romper el CRUD */ }
    }

}