using MetaCrudFosis.Api.Models;

namespace MetaCrudFosis.Api.Repositories;

public interface ILogRepository
{
    SystemLog Add(string level, string message);
    SystemLog Add(string level, string message, string? source, string? details);
    IEnumerable<SystemLog> GetAll();
}