using System.Net.Http.Json;

namespace MetaCrudFosis.Generator.Services;

public class FileMutationService
{
    private readonly TemplateEngineService _engine;
    private readonly IHttpClientFactory _httpClientFactory;

    public FileMutationService(TemplateEngineService engine, IHttpClientFactory httpClientFactory)
    {
        _engine = engine;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Sube carpetas desde el directorio de ejecución hasta encontrar la carpeta "src"
    /// (el ancla de la solución). Devuelve la ruta absoluta de "src".
    /// </summary>
    private static string LocalizarSrc()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var src = Path.Combine(dir.FullName, "src");
            if (Directory.Exists(src) &&
                Directory.Exists(Path.Combine(src, "MetaCrudFosis.Api")))
                return src;
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException(
            "No se encontró la carpeta 'src' de la solución subiendo desde " + AppContext.BaseDirectory);
    }

    public record ResultadoGeneracion(bool Ok, string Mensaje, List<string> ArchivosCreados);

    public async Task<ResultadoGeneracion> GenerarAsync(string entity, string corpColor, List<Campo> campos)
    {
        var creados = new List<string>();
        try
        {
            var src = LocalizarSrc();

            // Rutas de destino
            var apiModel = Path.Combine(src, "MetaCrudFosis.Api", "Models", $"{entity}.cs");
            var webModel = Path.Combine(src, "MetaCrudFosis.Web", "Models", $"{entity}.cs");
            var webCtrl = Path.Combine(src, "MetaCrudFosis.Web", "Controllers", $"{entity}Controller.cs");
            var webViewDir = Path.Combine(src, "MetaCrudFosis.Web", "Views", entity);
            var webViewIdx = Path.Combine(webViewDir, "Index.cshtml");

            // Generar contenido (motor 3B)
            var contenidoApiModel = _engine.GenerarModeloApi(entity, campos);
            var contenidoWebModel = _engine.GenerarModeloWeb(entity, campos);
            var contenidoWebCtrl = _engine.GenerarControladorWeb(entity);
            var contenidoWebIndex = _engine.GenerarVistaIndex(entity, corpColor, campos);

            // Crear carpeta de vistas si no existe
            Directory.CreateDirectory(webViewDir);

            // Escribir archivos
            await File.WriteAllTextAsync(apiModel, contenidoApiModel); creados.Add(apiModel);
            await File.WriteAllTextAsync(webModel, contenidoWebModel); creados.Add(webModel);
            await File.WriteAllTextAsync(webCtrl, contenidoWebCtrl); creados.Add(webCtrl);
            await File.WriteAllTextAsync(webViewIdx, contenidoWebIndex); creados.Add(webViewIdx);

            // Registrar SUCCESS en el log de la API (NoSQL). No bloqueante: si la API
            // está apagada, el log falla en silencio pero la generación se da por buena.
            await RegistrarLogAsync(entity, campos.Count);

            return new ResultadoGeneracion(true,
                $"✓ Entidad '{entity}' generada: {creados.Count} archivos creados.", creados);
        }
        catch (Exception ex)
        {
            return new ResultadoGeneracion(false, $"✗ Error: {ex.Message}", creados);
        }
    }

    private async Task RegistrarLogAsync(string entity, int numCampos)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5001/");
            await client.PostAsJsonAsync("api/Log", new
            {
                level = "SUCCESS",
                message = $"Entidad {entity} generada con {numCampos} campos."
            });
        }
        catch
        {
            // La API puede estar apagada al generar; no es un fallo de la generación.
        }
    }
}