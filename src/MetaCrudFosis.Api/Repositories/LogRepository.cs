using LiteDB;
using MetaCrudFosis.Api.Models;

namespace MetaCrudFosis.Api.Repositories;

/// <summary>
/// Implementación del repositorio de auditoría sobre LiteDB (NoSQL).
/// Persiste en el archivo local "logs.db". LiteDB crea el archivo y la colección
/// al vuelo si no existen, y autoincrementa el Id entero automáticamente.
/// La conexión se abre y cierra en cada operación (using) para liberar el archivo.
/// </summary>
public class LogRepository : ILogRepository
{
    private readonly string _connectionString;

    public LogRepository(IConfiguration configuration)
    {
        // Toma la cadena de conexión de la configuración; si no está, usa un valor por defecto.
        // "Connection=shared" permite que varios procesos accedan al mismo archivo.
        _connectionString = configuration.GetConnectionString("LiteDb") ?? "Filename=logs.db;Connection=shared";
    }

    // Sobrecarga corta: log sin origen ni detalle.
    public SystemLog Add(string level, string message)
        => Add(level, message, null, null);

    // Registra un log completo. El Timestamp lo pone SIEMPRE el servidor (no el cliente).
    public SystemLog Add(string level, string message, string? source, string? details)
    {
        using var db = new LiteDatabase(_connectionString);
        var logs = db.GetCollection<SystemLog>("logs");

        var entry = new SystemLog
        {
            Level = level,
            Message = message,
            Source = source,
            Details = details,
            Timestamp = DateTime.Now
        };

        logs.Insert(entry);   // LiteDB asigna el Id automáticamente
        return entry;
    }

    public IEnumerable<SystemLog> GetAll()
    {
        using var db = new LiteDatabase(_connectionString);
        return db.GetCollection<SystemLog>("logs").FindAll().ToList();
    }
}