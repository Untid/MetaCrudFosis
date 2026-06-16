var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// HttpClient con nombre "Api" apuntando a la API REST.
// IMPORTANTE: usa http (no https) y barra final en BaseAddress, para que las rutas
// relativas ("api/Producto") se compongan correctamente.
builder.Services.AddHttpClient("Api", client =>
    client.BaseAddress = new Uri("http://localhost:5001/"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();   // sirve recursos estáticos, incluido el CSS de la RCL (_content/...)

// Ruta por defecto: la entidad Producto es la página de inicio de la Web.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Producto}/{action=Index}/{id?}");

app.Run();