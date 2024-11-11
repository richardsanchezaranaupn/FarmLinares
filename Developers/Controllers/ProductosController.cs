using ClosedXML.Excel;
using Developers.Models;
using Developers.Models.ViewModels;
using Developers.Repositories.Interfaces;
using Developers.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NuGet.DependencyResolver;

namespace Developers.Controllers;
[Authorize(Roles = DS.Role_Admin + "," + DS.Role_Empleado)]

public class ProductosController : Controller
{
    private readonly IUnitWork _unitWork;
    public ProductosController(IUnitWork unitWork)
    {
        _unitWork = unitWork;
    }

    [HttpGet]

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Inventory()
    {
        return View();
    }

    [HttpGet] //Mostrar el formulario de creación
    public IActionResult Create()
    {
        ProductoVM productoVM = new ProductoVM()
        {
            Producto = new Models.Producto(),
            CategoriaList = _unitWork.Producto.ObtenerTodosDropdownLista("Categoria"),
            ProveedorList = _unitWork.Producto.ObtenerTodosDropdownLista("Proveedor")
        };
        return View(productoVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductoVM productoVM)
    {
        if (productoVM == null) return NotFound();

        // Imprimir los valores seleccionados en la consola para depuración
        Console.WriteLine($"CategoriaId: {productoVM.Producto.CategoriaId}, ProveedorId: {productoVM.Producto.ProveedorId}");

        // Verificar que los ID de categoría y proveedor sean válidos
        if (productoVM.Producto.CategoriaId == 0 || productoVM.Producto.ProveedorId == 0)
        {
            TempData[DS.Error] = "Debe seleccionar una categoría y un proveedor válidos.";
            productoVM.CategoriaList = _unitWork.Producto.ObtenerTodosDropdownLista("Categoria");
            productoVM.ProveedorList = _unitWork.Producto.ObtenerTodosDropdownLista("Proveedor");
            return View(productoVM);
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _unitWork.Producto.AgregarAsync(productoVM.Producto);
                await _unitWork.GuardarAsync();
                TempData[DS.Successfull] = "Producto ingresado al almacén correctamente.";
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                TempData[DS.Error] = "Error al crear el producto: Verifique que la categoría y el proveedor sean válidos.";
                Console.WriteLine($"Error de SQL al insertar producto: {ex.Message}");
            }
        }

        productoVM.CategoriaList = _unitWork.Producto.ObtenerTodosDropdownLista("Categoria");
        productoVM.ProveedorList = _unitWork.Producto.ObtenerTodosDropdownLista("Proveedor");
        TempData[DS.Error] = "Error al guardar producto, intente de nuevo.";
        return View(productoVM);
    }



    [HttpGet]
    public async Task<ActionResult<int>> GetCantidadDisponible(int productoId)
    {
        try
        {
            var producto = await _unitWork.Producto.ObtenerAsync(productoId);

            if (producto != null)
            {
                int cantidadDisponible = producto.Cantidad;
                return Json(cantidadDisponible);
            }
            else
            {
                return BadRequest("Producto no encontrado");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener la cantidad disponible del producto con ID: {productoId}. Detalles: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<int>> GetStock(int productoId)
    {
        try
        {
            var producto = await _unitWork.Producto.ObtenerAsync(productoId);

            if (producto != null)
            {
                int stock = producto.Cantidad;
                return Json(stock);
            }
            else
            {
                return BadRequest("Producto no encontrado");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener el stock del producto con ID: {productoId}. Detalles: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetReceta(int productoId)
    {
        var producto = await _unitWork.Producto.ObtenerPrimeroAsync(p => p.ProductoId == productoId);
        if (producto != null)
        {
            return Ok(producto.Receta); // Ajusta según el nombre real del campo de receta
        }
        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetPrecioUnitario(int productoId)
    {
        var producto = await _unitWork.Producto.ObtenerPrimeroAsync(p => p.ProductoId == productoId);
        if (producto != null)
        {
            return Ok(producto.Precio); // Ajusta según el nombre real del campo de precio unitario
        }
        return NotFound();
    }


    [HttpGet]
    public async Task<IActionResult> Vista(int? id)
    {
        if (id is null)
            return NotFound();

        var producto = await _unitWork.Producto.ObtenerAsync(id.GetValueOrDefault());

        if (producto is null)
            return NotFound();

        // Cargar las listas necesarias para la vista, si es necesario
        ProductoVM productoVM = new ProductoVM()
        {
            Producto = producto,
            CategoriaList = _unitWork.Producto.ObtenerTodosDropdownLista("Categoria"),
            ProveedorList = _unitWork.Producto.ObtenerTodosDropdownLista("Proveedor")
        };

        return View(productoVM);
    }

    //[HttpGet]
    //public async Task<IActionResult> Edit(int? id)
    //{
    //    if (id == null)
    //        return NotFound();

    //    // Obtener el producto con base en el ID
    //    var producto = await _unitWork.Producto.ObtenerAsync(id.Value);
    //    // Asegúrate de incluir el proveedor
    //    producto.Proveedor = await _unitWork.Proveedor.ObtenerAsync(producto.ProveedorId);
    //    // Asegúrate de incluir el proveedor
    //    producto.Categoria = await _unitWork.Categoria.ObtenerAsync(producto.CategoriaId);

    //    if (producto == null)
    //        return NotFound();

    //    // Crear una instancia de ProductoVM con los datos del producto
    //    var productoVM = new ProductoVM
    //    {
    //        Producto = producto,
    //        CategoriaList = _unitWork.Producto.ObtenerTodosDropdownLista("Categoria"), // Lista de categorías
    //        ProveedorList = _unitWork.Producto.ObtenerTodosDropdownLista("Proveedor")  // Lista de proveedores
    //    };

    //    return View(productoVM);  // Devolver la vista con el modelo ProductoVM
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(ProductoVM productoVM)
    //{
    //    if (productoVM == null)
    //        return NotFound();

    //    // Imprimir los valores seleccionados para depuración
    //    Console.WriteLine($"CategoriaId: {productoVM.Producto.CategoriaId}, ProveedorId: {productoVM.Producto.ProveedorId}");

    //    // Verificar que los ID de categoría y proveedor sean válidos
    //    if (productoVM.Producto.CategoriaId == 0 || productoVM.Producto.ProveedorId == 0)
    //    {
    //        TempData["Error"] = "Debe seleccionar una categoría y un proveedor válidos.";

    //        // Recargar las listas de categorías y proveedores en caso de error
    //        productoVM.CategoriaList = _unitWork.Producto.ObtenerTodosDropdownLista("Categoria");
    //        productoVM.ProveedorList = _unitWork.Producto.ObtenerTodosDropdownLista("Proveedor");
    //        return View(productoVM);
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            // Actualizar el producto en la base de datos
    //            _unitWork.Producto.Actualizar(productoVM.Producto);
    //            await _unitWork.GuardarAsync();

    //            TempData["Successful"] = "Producto actualizado correctamente.";
    //            return RedirectToAction("Index");
    //        }
    //        catch (SqlException ex)
    //        {
    //            TempData["Error"] = "Error al actualizar el producto: Verifique los datos ingresados.";
    //            Console.WriteLine($"Error de SQL al actualizar producto: {ex.Message}");
    //        }
    //    }

    //    // Si hay un error o los datos no son válidos, recargar las listas
    //    productoVM.CategoriaList = _unitWork.Producto.ObtenerTodosDropdownLista("Categoria");
    //    productoVM.ProveedorList = _unitWork.Producto.ObtenerTodosDropdownLista("Proveedor");

    //    TempData["Error"] = "Error al actualizar producto, intente de nuevo.";
    //    return View(productoVM);
    //}

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
            return NotFound();

        var producto = await _unitWork.Producto.ObtenerAsync(id.GetValueOrDefault());

        if (producto is null)
            return NotFound();

        // Verifica y carga la categoría
        if (producto?.CategoriaId != null)
        {
            producto.Categoria = await _unitWork.Categoria.ObtenerAsync(producto.CategoriaId);
        }

        // Verifica y carga el proveedor
        if (producto?.ProveedorId != null)
        {
            producto.Proveedor = await _unitWork.Proveedor.ObtenerAsync(producto.ProveedorId);
        }

        return View(producto);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Producto producto)
    {
        if (ModelState.IsValid)
        {
            _unitWork.Producto.Actualizar(producto); // Asegúrate de que el método de actualización maneje las propiedades adecuadamente.
            await _unitWork.GuardarAsync();
            TempData["Successful"] = "Producto actualizado correctamente";
            return RedirectToAction("Inventory");
        }
        return View(producto);
    }


    [HttpGet]
    public async Task<IActionResult> ExportarExcel()
    {
        var productos = await _unitWork.Producto.ObtenerTodosAsync(includeProperties: "Categoria,Proveedor");

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Productos");

            // Título en la cabecera (abarcando solo 8 celdas)
            var titleCell = worksheet.Range("A1:H1").Merge().FirstCell();
            titleCell.Value = "PRODUCTOS EN EL REGISTRO DE ALMACEN";
            titleCell.Style.Font.Bold = true;
            titleCell.Style.Font.FontSize = 20;
            titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Encabezados de la tabla
            var headerRow = worksheet.Row(3); // Comienza en la fila 3 para dejar espacio para el título
            headerRow.Cell(1).Value = "ID";
            headerRow.Cell(2).Value = "Nombre";
            headerRow.Cell(3).Value = "Receta";
            headerRow.Cell(4).Value = "Cantidad";
            headerRow.Cell(5).Value = "Precio";
            headerRow.Cell(6).Value = "Fecha Ingreso";
            headerRow.Cell(7).Value = "Categoria";
            headerRow.Cell(8).Value = "Proveedor";

            // Datos de los productos
            int currentRow = 4; // Comienza en la fila 4 para dejar espacio para el título y encabezados
            foreach (var producto in productos)
            {
                worksheet.Cell(currentRow, 1).Value = producto.ProductoId;
                worksheet.Cell(currentRow, 2).Value = producto.ProductoName;
                worksheet.Cell(currentRow, 3).Value = producto.Receta;
                worksheet.Cell(currentRow, 4).Value = producto.Cantidad;
                worksheet.Cell(currentRow, 5).Value = producto.Precio;
                worksheet.Cell(currentRow, 6).Value = producto.FechaIngreso.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 7).Value = producto.Categoria?.CategoriaName;
                worksheet.Cell(currentRow, 8).Value = producto.Proveedor?.ProveedorName;
                currentRow++;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Productos.xlsx");
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> SearchCategory(string term)
    {
        if (!string.IsNullOrEmpty(term))
        {
            var categoryList = await _unitWork.Categoria.ObtenerTodosAsync(c => c.Status == true);
            var data = categoryList
                .Where(x => x.CategoriaName.Contains(term, StringComparison.OrdinalIgnoreCase)) // Filtrar por el término
                .Select(x => new { categoriaId = x.CategoriaId, categoriaName = x.CategoriaName })
                .ToList();
            return Json(data); // Retornar los datos en formato JSON
        }
        return Ok();
    }


    [HttpGet]
    public async Task<IActionResult> SearchProveedor(string term)
    {
        if (!string.IsNullOrEmpty(term))
        {
            var proveedorList = await _unitWork.Proveedor.ObtenerTodosAsync(p => p.Status == true);
            var data = proveedorList
                .Where(x => x.ProveedorName.Contains(term, StringComparison.OrdinalIgnoreCase)) // Filtrar por el término
                .Select(x => new { proveedorId = x.ProveedorId, proveedorName = x.ProveedorName })
                .ToList();
            return Json(data); // Retornar los datos en formato JSON
        }
        return Ok();
    }
    #region API
    /// <summary>
    /// Listar todos los productos registrados
    /// </summary>
    /// <returns>Json</returns>
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var productos = await _unitWork.Producto.ObtenerTodosAsync(
            includeProperties: "Categoria,Proveedor",
            orderBy: c => c.OrderByDescending(c => c.ProductoId),
            isTracking: false);

        return Json(new { data = productos });
    }

    /// <summary>
    /// Eliminar un registro enviado por Ajax
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Json</returns>
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var productosDB = await _unitWork.Producto.ObtenerAsync(id);

        if (productosDB is null) // Si es nulo muestre un mensaje
            return Json(new { success = false, message = "Error al eliminar el producto" });

        // Si eliminó correctamente muestre mensaje
        _unitWork.Producto.Remover(productosDB);
        await _unitWork.GuardarAsync();

        return Json(new { success = true, message = "Producto eliminado correctamente" });
    }
    #endregion


    
}
