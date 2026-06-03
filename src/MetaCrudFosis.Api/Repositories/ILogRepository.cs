using MetaCrudFosis.Api.Models;

namespace MetaCrudFosis.Api.Repositories;

public interface ILogRepository
{
    SystemLog Add(string level, string message);
    IEnumerable<SystemLog> GetAll();
}