namespace MetaCrudFosis.Api.Models;

/// <summary>
/// Entidad de prueba MANUAL para validar el CRUD genérico (Fase 1).
/// En Fase 4, T4 generará entidades como esta (p.ej. Zapato.cs)
/// </summary>

public class Producto : IEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public DateTime FechaAlta { get; set; }
}
