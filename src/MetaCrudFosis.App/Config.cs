namespace MetaCrudFosis.App;

/// <summary>
/// Configuración centralizada de la URL de la Web MVC que el contenedor MAUI carga.
/// Es la única pieza de lógica propia de la app MAUI: el resto es un WebView que
/// simplemente muestra esta URL. Se usa compilación condicional porque el significado
/// de "localhost" cambia según la plataforma.
/// </summary>
public static class Config
{
    // - Windows (escritorio): "localhost" apunta al propio PC, donde corre la Web. Funciona directo.
    // - Android: "localhost" apunta al PROPIO móvil, NO al PC. Por eso se usa la IP del PC en la red local.
    //     · Emulador Android: usar 10.0.2.2 (alias del host que redirige al localhost del PC).
    //     · Dispositivo físico: usar la IP del PC (la "Dirección IPv4" de ipconfig, NO la puerta de enlace).
    //
    // IMPORTANTE: al cambiar de red WiFi (p. ej. en la presentación), la IP del PC cambia
    // y hay que actualizar este valor y recompilar la app.
#if ANDROID
    public const string WebUrl = "http://192.168.1.161:5002";   // IP del PC en la red local (ajustar por red)
#else
    public const string WebUrl = "http://localhost:5002";
#endif
}