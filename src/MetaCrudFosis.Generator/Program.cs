var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseDefaultFiles();   // sirve index.html como página por defecto
app.UseStaticFiles();    // sirve wwwroot (XP.css, el html, el js)

// Endpoint de generación: por ahora responde un placeholder. En la 3C hará la mutación real.
app.MapPost("/api/generar", (GeneracionRequest req) =>
{
    // TODO 3C: procesar plantillas y escribir archivos.
    return Results.Ok(new { ok = true, mensaje = $"(Placeholder) Recibido: {req.EntityName} con {req.Fields?.Count ?? 0} campos." });
});

app.Run();

// Modelo de entrada del formulario (lo usaremos de verdad en la 3C)
record CampoDto(string Name, string Type);
record GeneracionRequest(string EntityName, string CorpColor, List<CampoDto> Fields);