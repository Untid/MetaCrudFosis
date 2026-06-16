using System.Net.Http.Json;

namespace MetaCrudFosis.Generator.Services;

/// <summary>
/// Orquestador de la generación: coordina al motor de plantillas (TemplateEngineService)
/// y escribe físicamente los archivos resultantes en los proyectos de la solución.
/// Es el responsable de la "mutación en vivo": localiza la carpeta src, genera el contenido
/// de cada archivo y lo persiste en disco. Por entidad escribe 6 archivos (modelos, controlador
/// y vistas). Importante: solo CREA archivos nuevos, nunca edita existentes, reduciendo el
/// riesgo de romper el código ya funcional.
/// </summary>
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
    /// Localiza la carpeta "src" de la solución subiendo desde el directorio de ejecución.
    /// La identifica comprobando que contenga el proyecto MetaCrudFosis.Api. Así el generador
    /// funciona sin rutas absolutas fijas, independientemente de dónde se ejecute.
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

    // Resultado de la operación: éxito/fallo, mensaje para la UI y lista de archivos creados.
    public record ResultadoGeneracion(bool Ok, string Mensaje, List<string> ArchivosCreados);

    public async Task<ResultadoGeneracion> GenerarAsync(string entity, string corpColor, List<Campo> campos)
    {
        var creados = new List<string>();
        try
        {
            var src = LocalizarSrc();

            // Rutas de destino de los 6 archivos a generar.
            var apiModel = Path.Combine(src, "MetaCrudFosis.Api", "Models", $"{entity}.cs");
            var webModel = Path.Combine(src, "MetaCrudFosis.Web", "Models", $"{entity}.cs");
            var webCtrl = Path.Combine(src, "MetaCrudFosis.Web", "Controllers", $"{entity}Controller.cs");
            var webViewDir = Path.Combine(src, "MetaCrudFosis.Web", "Views", entity);
            var webViewIdx = Path.Combine(webViewDir, "Index.cshtml");
            var webViewCreate = Path.Combine(webViewDir, "Create.cshtml");
            var webViewEdit = Path.Combine(webViewDir, "Edit.cshtml");

            // Generación del contenido mediante el motor de plantillas.
            var contenidoApiModel = _engine.GenerarModeloApi(entity, campos);
            var contenidoWebModel = _engine.GenerarModeloWeb(entity, campos);
            var contenidoWebCtrl = _engine.GenerarControladorWeb(entity);
            var contenidoWebIndex = _engine.GenerarVistaIndex(entity, corpColor, campos);
            var contenidoCreate = _engine.GenerarVistaCreate(entity, corpColor, campos);
            var contenidoEdit = _engine.GenerarVistaEdit(entity, corpColor, campos);

            // Crea la carpeta de vistas de la entidad si no existe.
            Directory.CreateDirectory(webViewDir);

            // Escritura física de los archivos.
            await File.WriteAllTextAsync(apiModel, contenidoApiModel); creados.Add(apiModel);
            await File.WriteAllTextAsync(webModel, contenidoWebModel); creados.Add(webModel);
            await File.WriteAllTextAsync(webCtrl, contenidoWebCtrl); creados.Add(webCtrl);
            await File.WriteAllTextAsync(webViewIdx, contenidoWebIndex); creados.Add(webViewIdx);
            await File.WriteAllTextAsync(webViewCreate, contenidoCreate); creados.Add(webViewCreate);
            await File.WriteAllTextAsync(webViewEdit, contenidoEdit); creados.Add(webViewEdit);

            // Registra el éxito en el log de la API (NoSQL). Es no bloqueante: si la API está
            // apagada, el log falla en silencio pero la generación se considera correcta igualmente.
            await RegistrarLogAsync(entity, campos.Count);

            return new ResultadoGeneracion(true,
                $"✓ Entidad '{entity}' generada: {creados.Count} archivos creados.", creados);
        }
        catch (Exception ex)
        {
            // Si algo falla, se devuelve el error y la lista de lo que sí se llegó a crear.
            return new ResultadoGeneracion(false, $"✗ Error: {ex.Message}", creados);
        }
    }

    // Envía un log de nivel SUCCESS a la API. Envuelto en try/catch porque la API puede no
    // estar levantada en el momento de generar, y eso no debe contar como fallo de generación.
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