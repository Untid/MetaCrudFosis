using MetaCrudFosis.Api.Data;
using MetaCrudFosis.Api.Infrastructure;
using MetaCrudFosis.Api.Middlewares;
using MetaCrudFosis.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- Conexión a la base de datos relacional (SQLite) vía EF Core ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Inyección de dependencias ---
// El repositorio genérico se registra como tipo abierto: una sola línea sirve a
// IGenericRepository<Producto>, <Zapato>, etc. El repositorio de logs es Singleton
// porque LiteDB gestiona su propio acceso al archivo.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddSingleton<ILogRepository, LogRepository>();

// --- CORS ---
// En desarrollo se permite cualquier origen para que el cliente móvil (que llega
// desde la IP del PC, un origen distinto) pueda consumir la API. En producción
// se restringiría a los orígenes concretos autorizados.
builder.Services.AddCors(options =>
    options.AddPolicy("PermitirWeb", policy =>
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()));

// --- Registro de controladores genéricos (el corazón del descubrimiento dinámico) ---
// Estas dos configuraciones DEBEN ir encadenadas:
//  - RouteConvention: hace que la ruta use el nombre de la entidad (/api/Producto).
//  - FeatureProvider: enseña a ASP.NET a instanciar GenericController<T> por cada entidad.
// Sin esto, ASP.NET no descubre los controladores genéricos y las rutas dan 404.
builder.Services
    .AddControllers(options =>
        options.Conventions.Add(new GenericControllerRouteConvention()))
    .ConfigureApplicationPartManager(manager =>
        manager.FeatureProviders.Add(new GenericControllerFeatureProvider()));

builder.Services.AddOpenApi();   // documentación OpenAPI nativa de .NET 10

var app = builder.Build();

// --- Creación incremental de tablas (mejora clave de la Fase 4) ---
// En lugar de recrear la BD al añadir entidades, se genera el script del esquema y
// se adapta a "CREATE TABLE IF NOT EXISTS". Así, al generar una entidad nueva, su
// tabla aparece sin borrar la base de datos ni perder los datos ya almacenados.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var creator = db.GetService<Microsoft.EntityFrameworkCore.Storage.IRelationalDatabaseCreator>();
    creator.EnsureCreated();   // asegura que el archivo .db existe

    var script = db.Database.GenerateCreateScript();

    // SQLite: convertir cada "CREATE TABLE" en "CREATE TABLE IF NOT EXISTS"
    script = script.Replace("CREATE TABLE \"", "CREATE TABLE IF NOT EXISTS \"");

    db.Database.ExecuteSqlRaw(script);
}

// --- Documentación interactiva (solo en desarrollo) ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();   // interfaz Scalar en /scalar/v1
}

// El middleware global de excepciones se registra el primero para envolver toda la tubería.
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("PermitirWeb");

app.MapControllers();

app.Run();