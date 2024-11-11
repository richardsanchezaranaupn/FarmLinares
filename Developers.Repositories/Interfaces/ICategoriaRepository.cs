using Developers.Models;

namespace Developers.Repositories.Interfaces;

public interface ICategoriaRepository : IRepositoryBase<Categoria>
{
    void Actualizar(Categoria categoria);
}
