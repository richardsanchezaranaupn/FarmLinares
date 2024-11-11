using Developers.Persistence;
using Developers.Repositories.Interfaces;

namespace Developers.Repositories.Implementations;

public class UnitWork : IUnitWork
{
    private readonly DevelopersDbContext _db;
    public IProductoRepository Producto { get; private set; }
    public ICategoriaRepository Categoria { get; private set; }
    public IProveedorRepository Proveedor { get; private set; }
    public IApplicationUserRepository ApplicationUser { get; private set; }
    public IVentaRepository Venta { get; private set; }
    public IVentaDetalleRepository VentaDetalle { get; private set; }
    public UnitWork(DevelopersDbContext db)
    {
        _db = db;
        Producto = new ProductoRepository(_db);
        Categoria = new CategoriaRepository(_db);
        Proveedor = new ProveedorRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);
        Venta = new VentaRepository(_db);
        VentaDetalle = new VentaDetalleRepository(_db);
    }
    public void Dispose()
    {
        _db.Dispose();
    }

    public async Task GuardarAsync()
    {
        await _db.SaveChangesAsync();
    }
}
