using System.Net.Http.Json;
using MetaCrudFosis.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetaCrudFosis.Web.Controllers;

public class ProductoController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductoController(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;

    private HttpClient Api => _httpClientFactory.CreateClient("Api");

    // GET /Producto  → lista (servidor consume la API por HttpClient)
    public async Task<IActionResult> Index()
    {
        var productos = await Api.GetFromJsonAsync<List<Producto>>("api/Producto")
                        ?? new List<Producto>();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create() => View(new Producto { FechaAlta = DateTime.Today });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Producto model)
    {
        if (!ModelState.IsValid) return View(model);

        var resp = await Api.PostAsJsonAsync("api/Producto", model);
        if (!resp.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "La API rechazó la creación.");
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var producto = await Api.GetFromJsonAsync<Producto>($"api/Producto/{id}");
        return producto is null ? NotFound() : View(producto);
    }

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