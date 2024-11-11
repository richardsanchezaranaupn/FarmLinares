using Developers.Models;
using Developers.Persistence;
using Developers.Repositories.Interfaces;


namespace Developers.Repositories.Implementations;

public class ProveedorRepository : RepositoryBase<Proveedor>, IProveedorRepository
{
    private readonly DevelopersDbContext _db;
    public ProveedorRepository(DevelopersDbContext db) : base(db)
    {
        _db = db;
    }

    public void Actualizar(Proveedor proveedor)
    {
        var proveedorDB = _db.Proveedores.FirstOrDefault(t => t.ProveedorId == proveedor.ProveedorId);

        if (proveedorDB is not null)
        {
            proveedorDB.ProveedorName = proveedor.ProveedorName;
            proveedorDB.Ruc = proveedor.Ruc;
            proveedorDB.Direccion = proveedor.Direccion;
            proveedorDB.NombreContacto = proveedor.NombreContacto;
            proveedorDB.Telefono = proveedor.Telefono;
            proveedorDB.CorreoElectronico = proveedor.CorreoElectronico;

            proveedorDB.UpdatedAt = DateTime.Now;
            proveedorDB.Status = proveedor.Status;

            _db.SaveChanges();
        }
    }
}

