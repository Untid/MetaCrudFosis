namespace MetaCrudFosis.App;

public static class Config
{
    // URL de la Web MVC.
    // - Windows escritorio: localhost funciona directo.
    // - Android: localhost apunta al PROPIO móvil/emulador, NO a tu PC.
    //   · Emulador Android estándar: usa 10.0.2.2 (alias del host).
    //   · Dispositivo físico: usa la IP de tu PC en la red local (ej. 192.168.1.40).
#if ANDROID
    // Emulador: 10.0.2.2 redirige al localhost del PC.
    // Si usas móvil físico, cámbialo por la IP de tu PC: "http://192.168.1.XX:5002"
    public const string WebUrl = "http://192.168.1.161:5002";
#else
    public const string WebUrl = "http://localhost:5002";
#endif
}