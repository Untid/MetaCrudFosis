using Microsoft.AspNetCore.Mvc;

namespace MetaCrudFosis.Web.ViewComponents;

/// <summary>
/// ViewComponent que construye dinámicamente el menú de navegación de entidades.
/// En lugar de mantener una lista fija de enlaces, inspecciona la carpeta Views y crea
/// una entrada por cada entidad que tenga su vista Index.cshtml. Así, cuando el generador
/// crea una entidad nueva, esta aparece automáticamente en el menú sin tocar el layout.
/// </summary>
public class NavEntidadesViewComponent : ViewComponent
{
    private readonly IWebHostEnvironment _env;

    // Carpetas de Views que NO son entidades de negocio y deben excluirse del menú.
    private static readonly string[] Excluidas = { "Shared", "Home", "Log" };

    public NavEntidadesViewComponent(IWebHostEnvironment env) => _env = env;

    public IViewComponentResult Invoke()
    {
        var viewsPath = Path.Combine(_env.ContentRootPath, "Views");
        var entidades = new List<string>();

        if (Directory.Exists(viewsPath))
        {
            // Una carpeta es una "entidad navegable" si no está excluida y tiene su Index.cshtml.
            entidades = Directory.GetDirectories(viewsPath)
                .Select(Path.GetFileName)
                .Where(nombre => nombre is not null
                                 && !Excluidas.Contains(nombre)
                                 && File.Exists(Path.Combine(viewsPath, nombre, "Index.cshtml")))
                .Select(nombre => nombre!)
                .OrderBy(n => n)
                .ToList();
        }

        return View(entidades);
    }
}