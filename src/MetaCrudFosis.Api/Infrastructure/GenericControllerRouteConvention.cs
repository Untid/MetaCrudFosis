using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MetaCrudFosis.Api.Infrastructure;

public class GenericControllerRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (!controller.ControllerType.IsGenericType) return;

        var entityType = controller.ControllerType.GenericTypeArguments[0];
        controller.ControllerName = entityType.Name; // [controller] → "Producto"
    }
}