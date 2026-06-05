using MetaCrudFosis.Api.Models;
using MetaCrudFosis.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace MetaCrudFosis.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenericController<T> : ControllerBase where T : class, IEntity
{
    private readonly IGenericRepository<T> _repository;
    private readonly ILogRepository _logs;

    public GenericController(IGenericRepository<T> repository, ILogRepository logs)
    {
        _repository = repository;
        _logs = logs;
    }

    // GET /api/Producto                  → todos
    // GET /api/Producto?nombre=Teclado    → filtra por nombre
    // GET /api/Producto?stock=15&precio=79.99 → varios filtros (AND)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        var filters = Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
        return filters.Count == 0
            ? Ok(await _repository.GetAllAsync())
            : Ok(await _repository.FindAsync(filters));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<T>> GetById(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<T>> Create([FromBody] T entity)
    {
        var created = await _repository.AddAsync(entity);
        _logs.Add("INFO", $"Entidad {typeof(T).Name} creada (Id={created.Id}).");
        return Created($"/api/{typeof(T).Name}/{created.Id}", created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] T entity)
    {
        if (id != entity.Id)
            return BadRequest("El Id de la ruta no coincide con el del cuerpo.");

        var ok = await _repository.UpdateAsync(entity);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _repository.DeleteAsync(id);
        if (!ok) return NotFound();

        _logs.Add("INFO", $"Entidad {typeof(T).Name} eliminada (Id={id}).");
        return NoContent();
    }

    [HttpGet("export/json")]
    public async Task<IActionResult> ExportJson()
    {
        var data = await _repository.GetAllAsync();
        var json = JsonSerializer.SerializeToUtf8Bytes(data,
            new JsonSerializerOptions { WriteIndented = true });

        return File(json, "application/json", $"{typeof(T).Name}.json");
    }

    [HttpGet("export/xml")]
    public async Task<IActionResult> ExportXml()
    {
        var data = (await _repository.GetAllAsync()).ToList();

        var serializer = new XmlSerializer(typeof(List<T>));
        using var stream = new MemoryStream();
        serializer.Serialize(stream, data);

        return File(stream.ToArray(), "application/xml", $"{typeof(T).Name}.xml");
    }

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
        return Ok(new { insertados });
    }

}