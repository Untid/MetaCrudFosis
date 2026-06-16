using MetaCrudFosis.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MetaCrudFosis.Api.Data;

/// <summary>
/// Contexto de Entity Framework Core. Su particularidad es que NO declara
/// un DbSet por entidad (no hay DbSet&lt;Producto&gt;, DbSet&lt;Zapato&gt;...).
/// En su lugar, descubre por Reflection TODAS las clases que implementan IEntity
/// y las registra automáticamente. Gracias a esto, el motor T4 solo tiene que
/// crear el archivo de la entidad: su tabla aparece sin tocar este contexto.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Busca en el ensamblado todas las clases concretas (no abstractas)
        // que implementen la interfaz IEntity.
        var entityTypes = typeof(IEntity).Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                        && typeof(IEntity).IsAssignableFrom(t));

        // Registra cada una en el modelo de EF Core, que creará su tabla.
        foreach (var type in entityTypes)
        {
            modelBuilder.Entity(type);
        }
        base.OnModelCreating(modelBuilder);
    }
}