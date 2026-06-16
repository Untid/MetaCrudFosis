using System.Reflection;
using MetaCrudFosis.Api.Controllers;
using MetaCrudFosis.Api.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MetaCrudFosis.Api.Infrastructure;

/// <summary>
/// Pieza clave del descubrimiento dinámico de controladores.
/// ASP.NET Core, por defecto, NO sabe instanciar un controlador genérico
/// GenericController&lt;T&gt;. Este FeatureProvider le indica explícitamente que,
/// por cada entidad IEntity encontrada, debe registrar una versión cerrada del
/// controlador (GenericController&lt;Producto&gt;, GenericController&lt;Zapato&gt;...).
/// Sin esta clase, las rutas /api/Producto devolverían 404.
/// </summary>
public class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        // Localiza todas las entidades concretas que implementan IEntity.
        var entityTypes = typeof(IEntity).Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                        && typeof(IEntity).IsAssignableFrom(t))
            .ToList();

        Console.WriteLine($">>> FeatureProvider OK. Entidades: {entityTypes.Count} ({string.Join(", ", entityTypes.Select(e => e.Name))})");

        foreach (var entityType in entityTypes)
        {
            // Construye en tiempo de ejecución el tipo cerrado GenericController<EntidadConcreta>.
            var controllerType = typeof(GenericController<>)
                .MakeGenericType(entityType)
                .GetTypeInfo();

            // Lo registra como controlador disponible (evitando duplicados).
            if (!feature.Controllers.Contains(controllerType))
                feature.Controllers.Add(controllerType);
        }
    }
}