using Microsoft.AspNetCore.Mvc;

namespace MetaCrudFosis.Web.ViewComponents;

public class NavEntidadesViewComponent : ViewComponent
{
    private readonly IWebHostEnvironment _env;
    private static readonly string[] Excluidas = { "Shared", "Home", "Log" };

    public NavEntidadesViewComponent(IWebHostEnvironment env) => _env = env;

    public IViewComponentResult Invoke()
    {
        var viewsPath = Path.Combine(_env.ContentRootPath, "Views");
        var entidades = new List<string>();

        if (Directory.Exists(viewsPath))
        {
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