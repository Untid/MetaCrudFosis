using System.Net.Http.Json;
using MetaCrudFosis.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetaCrudFosis.Web.Controllers;

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
        // Más recientes primero
        logs.Reverse();
        return View(logs);
    }
}