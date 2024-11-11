using Microsoft.AspNetCore.Mvc.Rendering;

namespace Developers.Models.ViewModels;

public class ProductoVM
{
    public Producto Producto { get; set; }
    public IEnumerable<SelectListItem>? CategoriaList { get; set; }
    public IEnumerable<SelectListItem>? ProveedorList { get; set; }
}
