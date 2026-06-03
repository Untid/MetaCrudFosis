using MetaCrudFosis.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MetaCrudFosis.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base (options)
    { 
    }

    // No declaramos DbSet<Producto> a propósito.
    // Auto-registramos por Reflection TODAS las clases que implementen IEntity.
    // Resultado: T4 solo crea el archivo de la entidad; la tabla aparece sola.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityTypes = typeof(IEntity).Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                        && typeof(IEntity).IsAssignableFrom(t));

        foreach(var type in entityTypes)
        {
            modelBuilder.Entity(type);
        }
        base.OnModelCreating(modelBuilder);
    }
}