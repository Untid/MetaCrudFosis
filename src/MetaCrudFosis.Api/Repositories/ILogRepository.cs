using MetaCrudFosis.Api.Models;

namespace MetaCrudFosis.Api.Repositories;

/// <summary>
/// Contrato del repositorio de auditoría (logs). Se define aparte del repositorio
/// genérico porque los logs viven en LiteDB (NoSQL), no en SQLite, y no siguen
/// el patrón CRUD de las entidades de negocio.
/// </summary>
public interface ILogRepository
{
    SystemLog Add(string level, string message);                                   // log simple
    SystemLog Add(string level, string message, string? source, string? details);  // log con detalle
    IEnumerable<SystemLog> GetAll();
}