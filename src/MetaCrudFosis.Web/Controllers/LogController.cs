using System.Net.Http.Json;
using MetaCrudFosis.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetaCrudFosis.Web.Controllers;

/// <summary>
/// Controlador MVC de la vista "Actividad" (logs). Es fijo, no generado por T4.
/// Consume el endpoint de logs de la API y los muestra en orden cronológico inverso
/// (los más recientes primero) para facilitar la consulta de la actividad reciente.
/// </summary>
public class LogController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LogController(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("Api");
        var logs = await client.GetFromJsonAsync<List<SystemLog>>("api/Log")
                   ?? new List<SystemLog>();
        logs.Reverse();   // la API los devuelve en orden de inserción; se muestran al revés
        return View(logs);
    }
}