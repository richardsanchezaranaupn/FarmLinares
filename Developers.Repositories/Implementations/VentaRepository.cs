using Developers.Models;
using Developers.Persistence;
using Developers.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Developers.Repositories.Implementations;

public class VentaRepository : RepositoryBase<Venta>, IVentaRepository
{
    private readonly DevelopersDbContext _db;

    public VentaRepository(DevelopersDbContext db) : base(db)
    {
        _db = db;
    }

    public void Actualizar(Venta venta)
    {
        var ventaDB = _db.Ventas.Include(v => v.VentaDetalles)
                                .FirstOrDefault(t => t.VentaId == venta.VentaId);

        if (ventaDB != null)
        {
            // Actualizar los datos de la venta
            ventaDB.FechaRegistro = venta.FechaRegistro;
            ventaDB.ClienteName = venta.ClienteName;
            ventaDB.UpdatedAt = DateTime.Now;
            ventaDB.Status = venta.Status;

            // Actualizar los detalles de la venta
            foreach (var detalle in venta.VentaDetalles)
            {
                var detalleDB = ventaDB.VentaDetalles.FirstOrDefault(d => d.VentaDetalleId == detalle.VentaDetalleId);

                if (detalleDB != null)
                {
                    // Si el detalle ya existe, actualizar sus valores
                    detalleDB.ProductoId = detalle.ProductoId;
                    detalleDB.Cantidad = detalle.Cantidad;
                    detalleDB.PrecioUnitario = detalle.PrecioUnitario;
                }
                else
                {
                    // Si el detalle no existe, agregarlo
                    ventaDB.VentaDetalles.Add(new VentaDetalle
                    {
                        ProductoId = detalle.ProductoId,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario
                    });
                }
            }

            // Eliminar los detalles que ya no están en la venta actualizada
            foreach (var detalleDB in ventaDB.VentaDetalles.ToList())
            {
                if (!venta.VentaDetalles.Any(d => d.VentaDetalleId == detalleDB.VentaDetalleId))
                {
                    _db.VentaDetalles.Remove(detalleDB);
                }
            }

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

    public async Task<Venta> ObtenerUltimoAsync()
    {
        return await _db.Ventas.OrderByDescending(v => v.VentaId).FirstOrDefaultAsync();
    }
}

