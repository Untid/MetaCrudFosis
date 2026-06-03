using System.Reflection;
using MetaCrudFosis.Api.Controllers;
using MetaCrudFosis.Api.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MetaCrudFosis.Api.Infrastructure;

public class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var entityTypes = typeof(IEntity).Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                        && typeof(IEntity).IsAssignableFrom(t))
            .ToList();

        // Traza temporal de diagnóstico (quítala cuando /api/Producto responda):
        Console.WriteLine($">>> FeatureProvider OK. Entidades: {entityTypes.Count} ({string.Join(", ", entityTypes.Select(e => e.Name))})");

        foreach (var entityType in entityTypes)
        {
            var controllerType = typeof(GenericController<>)
                .MakeGenericType(entityType)
                .GetTypeInfo();

            if (!feature.Controllers.Contains(controllerType))
                feature.Controllers.Add(controllerType);
        }
    }
}