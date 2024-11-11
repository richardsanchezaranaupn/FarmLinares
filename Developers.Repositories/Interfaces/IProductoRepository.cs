using Developers.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Developers.Repositories.Interfaces;

public interface IProductoRepository : IRepositoryBase<Producto>
{
    void Actualizar(Producto producto);
    IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);

}
