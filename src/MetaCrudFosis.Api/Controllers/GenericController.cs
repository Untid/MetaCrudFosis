using MetaCrudFosis.Api.Models;
using MetaCrudFosis.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MetaCrudFosis.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenericController<T> : ControllerBase where T : class, IEntity
{
    private readonly IGenericRepository<T> _repository;

    public GenericController(IGenericRepository<T> repository)
        => _repository = repository;

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
        return ok ? NoContent() : NotFound();
    }
}