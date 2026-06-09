using MetaCrudFosis.Generator.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

var templatesDir = Path.Combine(builder.Environment.ContentRootPath, "Templates");
var engine = new TemplateEngineService(templatesDir);

// PREVIEW 3B: devuelve el código generado como texto. NO escribe archivos.
app.MapPost("/api/preview", (GeneracionRequest req) =>
{
    var campos = (req.Fields ?? new()).Select(f => new Campo(f.Name, f.Type)).ToList();
    var sb = new System.Text.StringBuilder();
    sb.AppendLine("==== API Model (Models/" + req.EntityName + ".cs) ====");
    sb.AppendLine(engine.GenerarModeloApi(req.EntityName, campos));
    sb.AppendLine("\n==== Web Model (Models/" + req.EntityName + ".cs) ====");
    sb.AppendLine(engine.GenerarModeloWeb(req.EntityName, campos));
    sb.AppendLine("\n==== Web Controller (Controllers/" + req.EntityName + "Controller.cs) ====");
    sb.AppendLine(engine.GenerarControladorWeb(req.EntityName));
    sb.AppendLine("\n==== View (Views/" + req.EntityName + "/Index.cshtml) ====");
    sb.AppendLine(engine.GenerarVistaIndex(req.EntityName, req.CorpColor, campos));
    return Results.Text(sb.ToString(), "text/plain; charset=utf-8");
});

app.MapPost("/api/generar", (GeneracionRequest req) =>
    Results.Ok(new { ok = true, mensaje = $"(Placeholder) Recibido: {req.EntityName} con {req.Fields?.Count ?? 0} campos." }));

app.Run();

record CampoDto(string Name, string Type);
record GeneracionRequest(string EntityName, string CorpColor, List<CampoDto> Fields);