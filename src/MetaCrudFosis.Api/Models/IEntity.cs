namespace MetaCrudFosis.Api.Models;

/// <summary>
/// Contrato mínimo de toda entidad del sistema: solo exige un Id entero.
/// Es la pieza que habilita toda la magia genérica: la API descubre por Reflection
/// las clases que implementan esta interfaz para auto-registrar sus tablas (EF Core)
/// y sus controladores (FeatureProvider). Implementar IEntity es lo único que una
/// entidad necesita para obtener un CRUD completo automáticamente.
/// </summary>
public interface IEntity
{
    int Id { get; set; }
}