using MetaCrudFosis.Api.Models;

namespace MetaCrudFosis.Api.Repositories;

/// <summary>
/// Contrato del repositorio genérico de acceso a datos. Define las operaciones
/// CRUD (más el filtrado dinámico) para cualquier entidad que implemente IEntity.
/// Al programar contra esta interfaz, el controlador genérico no depende de la
/// implementación concreta (inversión de dependencias).
/// </summary>
public interface IGenericRepository<T> where T : class, IEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(IDictionary<string, string> filters);  // filtrado dinámico
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}