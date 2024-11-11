using Developers.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Developers.Repositories.Interfaces;

public interface IVentaDetalleRepository : IRepositoryBase<VentaDetalle>
{
    void Actualizar(VentaDetalle VentaDetalle);
    IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);
}
