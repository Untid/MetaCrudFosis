namespace MetaCrudFosis.Api.Models;

/// <summary>
/// Contrato mínimo de toda entidad del sistema.
/// La API la usa por Reflection para auto-registrar tablas y controladores.
/// </summary>

public interface IEntity
{
    int Id { get; set; }
}