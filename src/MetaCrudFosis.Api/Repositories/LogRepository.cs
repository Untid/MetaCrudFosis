using LiteDB;
using MetaCrudFosis.Api.Models;

namespace MetaCrudFosis.Api.Repositories;

/// <summary>
/// Repositorio NoSQL sobre LiteDB. Persiste en el archivo local "logs.db".
/// LiteDB autoincrementa el Id entero automáticamente al usar la colección.
/// </summary>
public class LogRepository : ILogRepository
{
    private readonly string _connectionString;

    public LogRepository(IConfiguration configuration)
    {
        // Reutiliza una ruta de archivo local; LiteDB la crea si no existe.
        _connectionString = configuration.GetConnectionString("LiteDb") ?? "Filename=logs.db;Connection=shared";
    }

    public SystemLog Add(string level, string message)
        => Add(level, message, null, null);

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

        logs.Insert(entry);
        return entry;
    }

    public IEnumerable<SystemLog> GetAll()
    {
        using var db = new LiteDatabase(_connectionString);
        return db.GetCollection<SystemLog>("logs").FindAll().ToList();
    }
}