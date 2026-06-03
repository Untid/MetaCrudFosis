using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using MetaCrudFosis.Api.Data;
using MetaCrudFosis.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MetaCrudFosis.Api.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.AsNoTracking().ToListAsync();

    /// <summary>
    /// Filtra por igualdad exacta usando los pares clave=valor del query string.
    /// Las claves se resuelven contra las propiedades de T por Reflection;
    /// el WHERE se construye como árbol de expresión y EF lo traduce a SQL.
    /// Claves desconocidas o valores no convertibles se ignoran (robusto ante entradas raras).
    /// </summary>
    public async Task<IEnumerable<T>> FindAsync(IDictionary<string, string> filters)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        foreach (var (key, rawValue) in filters)
        {
            var prop = typeof(T).GetProperty(key,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) continue;

            object typedValue;
            try
            {
                var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                typedValue = Convert.ChangeType(rawValue, targetType, CultureInfo.InvariantCulture);
            }
            catch
            {
                continue; // valor no convertible al tipo de la propiedad → se ignora
            }

            // Construye: x => x.Prop == typedValue
            var param = Expression.Parameter(typeof(T), "x");
            var member = Expression.Property(param, prop);
            var constant = Expression.Constant(typedValue, prop.PropertyType);
            var equals = Expression.Equal(member, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equals, param);

            query = query.Where(lambda);
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task<T> AddAsync(T entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null) return false;

        _dbSet.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}