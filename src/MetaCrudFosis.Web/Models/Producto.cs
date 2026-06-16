using System.ComponentModel.DataAnnotations;

namespace MetaCrudFosis.Web.Models;

/// <summary>
/// Modelo de Producto en la capa Web. A diferencia del modelo de la API, incorpora
/// anotaciones de validación y presentación (Data Annotations) que MVC usa para validar
/// el formulario en servidor y para mostrar etiquetas y formatos. Lo genera el motor T4.
/// </summary>
public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]   // validación: campo obligatorio
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Precio")]
    public decimal Precio { get; set; }

    [Display(Name = "Stock")]
    public int Stock { get; set; }

    [Display(Name = "Fecha de alta")]
    [DataType(DataType.Date)]                                                   // input de tipo fecha
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
    public DateTime FechaAlta { get; set; }
}