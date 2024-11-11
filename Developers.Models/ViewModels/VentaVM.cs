using Microsoft.AspNetCore.Mvc.Rendering;

namespace Developers.Models.ViewModels;

public class  VentaVM
{
    public Venta Venta { get; set; }
    public IEnumerable<SelectListItem>? ProductoList { get; set; }
    public List<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
    public int ProductoId { get; set; }
    public string ProductosDetalleJson { get; set; } // Campo para almacenar el JSON
}

