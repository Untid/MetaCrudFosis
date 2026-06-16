using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MetaCrudFosis.Api.Infrastructure;

/// <summary>
/// Convención de rutas para los controladores genéricos.
/// Sin esto, ASP.NET nombraría la ruta a partir del nombre interno del tipo
/// genérico (algo como "GenericController`1"), inservible como URL.
/// Esta convención fuerza que la ruta use el nombre de la entidad,
/// de modo que GenericController&lt;Producto&gt; quede expuesto como /api/Producto.
/// </summary>
public class GenericControllerRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        // Solo afecta a los controladores genéricos; el resto se deja intacto.
        if (!controller.ControllerType.IsGenericType) return;

        // Toma el tipo de entidad (el T) y usa su nombre como nombre de controlador.
        var entityType = controller.ControllerType.GenericTypeArguments[0];
        controller.ControllerName = entityType.Name; // [controller] → "Producto"
    }
}