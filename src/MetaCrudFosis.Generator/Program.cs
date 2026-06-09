using MetaCrudFosis.Generator.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();   // para el log a la API
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

var templatesDir = Path.Combine(builder.Environment.ContentRootPath, "Templates");
var engine = new TemplateEngineService(templatesDir);
var mutator = new FileMutationService(engine, app.Services.GetRequiredService<IHttpClientFactory>());

app.MapPost("/api/preview", (GeneracionRequest req) =>
{
    var campos = (req.Fields ?? new()).Select(f => new Campo(f.Name, f.Type)).ToList();
    var sb = new System.Text.StringBuilder();
    sb.AppendLine("==== API Model ===="); sb.AppendLine(engine.GenerarModeloApi(req.EntityName, campos));
    sb.AppendLine("\n==== Web Model ===="); sb.AppendLine(engine.GenerarModeloWeb(req.EntityName, campos));
    sb.AppendLine("\n==== Web Controller ===="); sb.AppendLine(engine.GenerarControladorWeb(req.EntityName));
    sb.AppendLine("\n==== View ===="); sb.AppendLine(engine.GenerarVistaIndex(req.EntityName, req.CorpColor, campos));
    return Results.Text(sb.ToString(), "text/plain; charset=utf-8");
});

// GENERACIÓN REAL: escribe los archivos en la solución.
app.MapPost("/api/generar", async (GeneracionRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.EntityName))
        return Results.BadRequest(new { ok = false, mensaje = "Falta el nombre de la entidad." });
    var campos = (req.Fields ?? new()).Select(f => new Campo(f.Name, f.Type)).ToList();
    if (campos.Count == 0)
        return Results.BadRequest(new { ok = false, mensaje = "Añade al menos un campo." });

    var resultado = await mutator.GenerarAsync(req.EntityName.Trim(), req.CorpColor, campos);
    return Results.Ok(new { ok = resultado.Ok, mensaje = resultado.Mensaje, archivos = resultado.ArchivosCreados });
});

app.Run();

record CampoDto(string Name, string Type);
record GeneracionRequest(string EntityName, string CorpColor, List<CampoDto> Fields);