using MetaCrudFosis.Api.Data;
using MetaCrudFosis.Api.Infrastructure;
using MetaCrudFosis.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// DDBB CONECTION
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de dedpendencias.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddSingleton<ILogRepository, LogRepository>();

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
    

app.MapControllers();

app.Run();