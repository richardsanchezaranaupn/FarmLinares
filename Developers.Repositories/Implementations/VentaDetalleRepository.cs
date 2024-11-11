using Developers.Models;
using Developers.Persistence;
using Developers.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Developers.Repositories.Implementations;

public class VentaDetalleRepository : RepositoryBase<VentaDetalle>, IVentaDetalleRepository
{
    private readonly DevelopersDbContext _db;
    public VentaDetalleRepository(DevelopersDbContext db) : base(db)
    {
        _db = db;
    }

    public void Actualizar(VentaDetalle ventaDetalle)
    {
        var detalleDB = _db.VentaDetalles.FirstOrDefault(d => d.VentaDetalleId == ventaDetalle.VentaDetalleId);

        if (detalleDB != null)
        {
            // Actualizar los datos del detalle de la venta
            detalleDB.ProductoId = ventaDetalle.ProductoId;
            detalleDB.Cantidad = ventaDetalle.Cantidad;
            detalleDB.PrecioUnitario = ventaDetalle.PrecioUnitario;

            // Guardar cambios
            _db.SaveChanges();
        }
    }

    public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
    {
        if (obj == "Producto")
            return _db.Productos.Where(c => c.Status == true).Select(c => new SelectListItem
            {
                Text = c.ProductoName,
                Value = c.ProductoId.ToString()
            });
        return null;
    }
}
