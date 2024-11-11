using Developers.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Developers.Repositories.Interfaces;

public interface IVentaRepository : IRepositoryBase<Venta>
{
    void Actualizar(Venta Venta);
    IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);
    Task<Venta> ObtenerUltimoAsync();
}
