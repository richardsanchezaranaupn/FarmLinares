using System.ComponentModel.DataAnnotations;

namespace Developers.Models;

public class Producto : EntityBase
{
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    public string ProductoName { get; set; }

    public int Cantidad { get; set; }

    [Range(0.01, 99999.99, ErrorMessage = "El precio debe estar entre 0.01 y 99999.99 con un máximo de 2 decimales.")]
    [RegularExpression(@"\d{1,5}(\.\d{1,2})?", ErrorMessage = "El precio debe tener un máximo de 5 dígitos y 2 decimales.")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El campo Receta es obligatorio.")]
    public string Receta { get; set; }

    public DateTime FechaIngreso { get; set; }

    // CATEGORIA
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    // PROVEEDOR
    public int ProveedorId { get; set; }
    public Proveedor? Proveedor { get; set; }
}

