namespace MetaCrudFosis.Api.Models;

/// <summary>
/// Entidad de ejemplo creada MANUALMENTE para validar el CRUD genérico (Fase 1).
/// Sirve de referencia y de molde: las entidades que genera el motor T4 (p. ej. Zapato.cs)
/// tienen exactamente esta forma. Al implementar IEntity, obtiene su tabla, su API REST
/// y sus vistas sin escribir código específico para ella.
/// </summary>
public class Producto : IEntity
{
    public int Id { get; set; }                           // clave primaria (autoincremental)
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public DateTime FechaAlta { get; set; }
}