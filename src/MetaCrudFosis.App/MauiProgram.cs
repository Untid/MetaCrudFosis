using Microsoft.Extensions.Logging;

namespace MetaCrudFosis.App;

/// <summary>
/// Punto de arranque y configuración de la app MAUI (equivalente al Program.cs del resto
/// de proyectos). Registra la app, las fuentes y el logging. Se mantiene minimalista:
/// la app no añade servicios ni lógica de negocio, porque su única función es alojar
/// el WebView que muestra la Web MVC.
/// </summary>
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();   // logging extra solo en compilación de depuración
#endif

        return builder.Build();
    }
}