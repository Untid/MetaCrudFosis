var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// HttpClient con nombre apuntando a la API. OJO: http, y barra final en BaseAddress.
builder.Services.AddHttpClient("Api", client =>
    client.BaseAddress = new Uri("http://localhost:5001/"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();   // sirve el CSS de la RCL (_content/...)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Producto}/{action=Index}/{id?}");

app.Run();