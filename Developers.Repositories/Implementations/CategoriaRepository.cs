using Developers.Models;
using Developers.Persistence;
using Developers.Repositories.Interfaces;


namespace Developers.Repositories.Implementations;

public class CategoriaRepository : RepositoryBase<Categoria>, ICategoriaRepository
{
    private readonly DevelopersDbContext _db;
    public CategoriaRepository(DevelopersDbContext db) : base(db)
    {
        _db = db;
    }

    public void Actualizar(Categoria categoria)
    {
        var categoriaDB = _db.Categorias.FirstOrDefault(t => t.CategoriaId == categoria.CategoriaId);

        if (categoriaDB is not null)
        {
            categoriaDB.CategoriaName = categoria.CategoriaName;
            categoriaDB.Descripcion = categoria.Descripcion;
            categoriaDB.UpdatedAt = DateTime.Now;
            categoriaDB.Status = categoria.Status;
            _db.SaveChanges();
        }
    }
}
