using System.Net.Http.Json;
using MetaCrudFosis.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetaCrudFosis.Web.Controllers;

/// <summary>
/// Controlador MVC de la entidad Producto. Es el tipo de controlador que el motor T4
/// genera por cada entidad (a diferencia del de la API, que es genérico). No accede a la
/// base de datos: actúa como cliente de la API REST, consumiéndola por HttpClient. Así se
/// mantiene la separación entre presentación (Web) y lógica/persistencia (API).
/// </summary>
public class ProductoController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductoController(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;

    // Cliente HTTP con nombre "Api", configurado en Program.cs apuntando a la API.
    private HttpClient Api => _httpClientFactory.CreateClient("Api");

    // GET /Producto → obtiene la lista desde la API y la pasa a la vista.
    public async Task<IActionResult> Index()
    {
        var productos = await Api.GetFromJsonAsync<List<Producto>>("api/Producto")
                        ?? new List<Producto>();
        return View(productos);
    }

    // GET /Producto/Create → muestra el formulario de alta, con la fecha de hoy por defecto.
    [HttpGet]
    public IActionResult Create() => View(new Producto { FechaAlta = DateTime.Today });

    // POST /Producto/Create → valida y envía el alta a la API.
    // ValidateAntiForgeryToken protege frente a CSRF en el envío del formulario.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Producto model)
    {
        if (!ModelState.IsValid) return View(model);   // re-muestra el form con errores de validación

        var resp = await Api.PostAsJsonAsync("api/Producto", model);
        if (!resp.IsSuccessStatusCode)
        {
            // Si la API rechaza la operación, se informa al usuario sin perder los datos del form.
            ModelState.AddModelError("", "La API rechazó la creación.");
            return View(model);
        }
        return RedirectToAction(nameof(Index));   // patrón POST-Redirect-GET
    }

    // GET /Producto/Edit/5 → recupera el producto de la API para editarlo.
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var producto = await Api.GetFromJsonAsync<Producto>($"api/Producto/{id}");
        return producto is null ? NotFound() : View(producto);
    }

    // POST /Producto/Edit → valida y envía la actualización a la API.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Producto model)
    {
        if (!ModelState.IsValid) return View(model);

        var resp = await Api.PutAsJsonAsync($"api/Producto/{model.Id}", model);
        if (!resp.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "La API rechazó la actualización.");
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }
}