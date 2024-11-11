using Developers.Models;

namespace Developers.Repositories.Interfaces;

public interface IProveedorRepository : IRepositoryBase<Proveedor>
{
    void Actualizar(Proveedor proveedor);
}
