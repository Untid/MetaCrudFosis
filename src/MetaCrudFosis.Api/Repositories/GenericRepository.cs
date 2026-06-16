using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using MetaCrudFosis.Api.Data;
using MetaCrudFosis.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MetaCrudFosis.Api.Repositories;

/// <summary>
/// Implementación genérica del acceso a datos sobre Entity Framework Core.
/// Una sola clase sirve a TODAS las entidades (T), evitando un repositorio por entidad.
/// Destaca el filtrado dinámico de FindAsync, construido con árboles de expresión.
/// </summary>
public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();   // EF resuelve el DbSet de T en tiempo de ejecución
    }

    // AsNoTracking: solo lectura, sin seguimiento de cambios → más rápido.
    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.AsNoTracking().ToListAsync();

    /// <summary>
    /// Filtrado dinámico a partir de los pares clave=valor del query string.
    /// Las claves se resuelven contra las propiedades de T por Reflection y el WHERE
    /// se construye como árbol de expresión que EF traduce a SQL. Según el tipo de la
    /// propiedad se aplica una estrategia distinta (texto parcial, fecha jerárquica,
    /// booleano humanizado, o igualdad exacta). Claves desconocidas o valores no
    /// convertibles se ignoran, haciendo el filtro robusto ante entradas inesperadas.
    /// </summary>
    public async Task<IEnumerable<T>> FindAsync(IDictionary<string, string> filters)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        foreach (var (key, rawValue) in filters)
        {
            var prop = typeof(T).GetProperty(key,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) continue;   // la entidad no tiene esa propiedad → se ignora

            var param = Expression.Parameter(typeof(T), "x");
            var member = Expression.Property(param, prop);
            Expression body;

            if (prop.PropertyType == typeof(string))
            {
                // Texto → búsqueda parcial (LIKE '%valor%')
                var contains = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
                body = Expression.Call(member, contains, Expression.Constant(rawValue, typeof(string)));
            }
            else if (prop.PropertyType == typeof(DateTime))
            {
                // Filtro jerárquico por año / año-mes / año-mes-día según las partes recibidas.
                // "2026" → todo el año; "2026-05" → ese mes; "2026-05-06" → ese día.
                var partes = rawValue.Split('-', StringSplitOptions.RemoveEmptyEntries);

                if (partes.Length == 0 || !int.TryParse(partes[0], out var anio))
                    continue; // valor no usable → se ignora este filtro

                var yearProp = typeof(DateTime).GetProperty(nameof(DateTime.Year))!;
                body = Expression.Equal(Expression.Property(member, yearProp),
                                        Expression.Constant(anio, typeof(int)));

                if (partes.Length >= 2 && int.TryParse(partes[1], out var mes) && mes is >= 1 and <= 12)
                {
                    var monthProp = typeof(DateTime).GetProperty(nameof(DateTime.Month))!;
                    var mesEq = Expression.Equal(Expression.Property(member, monthProp),
                                                 Expression.Constant(mes, typeof(int)));
                    body = Expression.AndAlso(body, mesEq);

                    if (partes.Length >= 3 && int.TryParse(partes[2], out var dia) && dia is >= 1 and <= 31)
                    {
                        var dayProp = typeof(DateTime).GetProperty(nameof(DateTime.Day))!;
                        var diaEq = Expression.Equal(Expression.Property(member, dayProp),
                                                     Expression.Constant(dia, typeof(int)));
                        body = Expression.AndAlso(body, diaEq);
                    }
                }
            }
            else if (prop.PropertyType == typeof(bool))
            {
                // Booleano → acepta valores humanos: sí/no, true/false, 1/0, verdadero/falso.
                var v = rawValue.Trim().ToLowerInvariant();
                bool? valorBool = v switch
                {
                    "true" or "sí" or "si" or "1" or "verdadero" => true,
                    "false" or "no" or "0" or "falso" => false,
                    _ => null
                };
                if (valorBool is null) continue; // no interpretable → se ignora el filtro

                body = Expression.Equal(member, Expression.Constant(valorBool.Value, typeof(bool)));
            }
            else
            {
                // int, decimal → igualdad exacta. Convert.ChangeType respeta cultura invariante.
                object typedValue;
                try
                {
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    typedValue = Convert.ChangeType(rawValue, targetType, CultureInfo.InvariantCulture);
                }
                catch { continue; }   // valor no convertible → se ignora el filtro

                body = Expression.Equal(member, Expression.Constant(typedValue, prop.PropertyType));
            }

            // Cada filtro se acumula como un WHERE adicional (combinación AND).
            query = query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task<T> AddAsync(T entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();   // persiste y asigna el Id autoincremental
        return entity;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return await _context.SaveChangesAsync() > 0;   // true si afectó a alguna fila
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null) return false;   // no existe → no se borra nada

        _dbSet.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}