using MetaCrudFosis.Api.Data;
using MetaCrudFosis.Api.Infrastructure;
using MetaCrudFosis.Api.Middlewares;
using MetaCrudFosis.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// DDBB CONECTION
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de dedpendencias.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddSingleton<ILogRepository, LogRepository>();

builder.Services.AddCors(options =>
    options.AddPolicy("PermitirWeb", policy =>
        policy.WithOrigins("http://localhost:5002")  // origen de la Web MVC. Ajustable.
              .AllowAnyHeader()
              .AllowAnyMethod()));

builder.Services
    .AddControllers(options =>
        options.Conventions.Add(new GenericControllerRouteConvention()))
    .ConfigureApplicationPartManager(manager =>
        manager.FeatureProviders.Add(new GenericControllerFeatureProvider()));

builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Creación incremental de tablas:
    // EF genera el script de creación del esquema; lo adaptamos a "IF NOT EXISTS"
    // para que cree solo las tablas que aún no existan, sin tocar datos existentes.
    // Así, al generar una entidad nueva, su tabla aparece sin borrar la BD.
    var creator = db.GetService<Microsoft.EntityFrameworkCore.Storage.IRelationalDatabaseCreator>();

    // Asegura que el archivo .db existe (no crea tablas)
    creator.EnsureCreated();

    var script = db.Database.GenerateCreateScript();

    // SQLite: convertir cada "CREATE TABLE" en "CREATE TABLE IF NOT EXISTS"
    script = script.Replace("CREATE TABLE \"", "CREATE TABLE IF NOT EXISTS \"");

    db.Database.ExecuteSqlRaw(script);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("PermitirWeb");

app.MapControllers();

app.Run();