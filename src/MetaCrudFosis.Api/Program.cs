using MetaCrudFosis.Api.Data;
using MetaCrudFosis.Api.Infrastructure;
using MetaCrudFosis.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// DDBB CONECTION
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// CLAVE: las dos llamadas van ENCADENADAS sobre el mismo IMvcBuilder.
// Si las separas, el FeatureProvider no se registra y MapControllers no ve nada (= 404).
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
    db.Database.EnsureCreated();
}

// Diagnóstico temporal (quítalo al validar):
app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> sources) =>
    sources.SelectMany(s => s.Endpoints)
           .OfType<RouteEndpoint>()
           .Select(e => e.RoutePattern.RawText)
           .OrderBy(x => x));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
    

app.MapControllers();

app.Run();