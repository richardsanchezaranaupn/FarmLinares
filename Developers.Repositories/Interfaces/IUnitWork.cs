namespace Developers.Repositories.Interfaces;

public interface IUnitWork : IDisposable
{
    // Agregar las Interfaces que se creen
    IProductoRepository Producto { get; }
    ICategoriaRepository Categoria { get; }
    IProveedorRepository Proveedor { get; }
    IApplicationUserRepository ApplicationUser { get; }
    IVentaRepository Venta {  get; }
    IVentaDetalleRepository VentaDetalle { get; }

    Task GuardarAsync();
}
