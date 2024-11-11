using Developers.Models;
using Developers.Persistence;
using Developers.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Developers.Repositories.Implementations;

public class ProductoRepository : RepositoryBase<Producto>, IProductoRepository
{
    private readonly DevelopersDbContext _db;
    public ProductoRepository(DevelopersDbContext db) : base(db)
    {
        _db = db;
    }

    public void Actualizar(Producto producto)
    {
        // Obtener el producto existente de la base de datos
        var productoDB = _db.Productos.FirstOrDefault(t => t.ProductoId == producto.ProductoId);

        // Verificar si se encontró el producto
        if (productoDB is not null)
        {
            // Actualizar los campos del producto
            productoDB.ProductoName = producto.ProductoName;
            productoDB.Receta = producto.Receta; // Asegúrate de que esta propiedad exista en tu modelo
            productoDB.Precio = producto.Precio;
            productoDB.Cantidad = producto.Cantidad;
            productoDB.FechaIngreso = producto.FechaIngreso;
            productoDB.CategoriaId = producto.CategoriaId; // Asegúrate de que este ID sea válido
            productoDB.ProveedorId = producto.ProveedorId; // Asegúrate de que este ID sea válido
            productoDB.UpdatedAt = DateTime.Now;
            productoDB.Status = producto.Status;

            try
            {
                // Guarda cambios en la base de datos
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Manejar la excepción (puedes loguear el error o manejarlo de otra manera)
                Console.WriteLine($"Error al actualizar el producto: {ex.Message}");
                throw; // Re-lanzar la excepción para su manejo en el controlador
            }
        }
        else
        {
            // Manejar el caso donde el producto no se encontró
            Console.WriteLine("Producto no encontrado para actualizar.");
        }
    }


    public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
    {
        if (obj == "Proveedor")
        {
            return _db.Proveedores
                .Where(p => p.Status == true)
                .Select(p => new SelectListItem
                {
                    Text = p.ProveedorName,
                    Value = p.ProveedorId.ToString()
                });
        }
        else if (obj == "Categoria")
        {
            return _db.Categorias
                .Where(c => c.Status == true) // Asegúrate de que estás filtrando por estado si es necesario
                .Select(c => new SelectListItem
                {
                    Text = c.CategoriaName,
                    Value = c.CategoriaId.ToString()
                });
        }
            
        return null; // Retornar null si obj no es ni Proveedor ni Categoria
    }



}
