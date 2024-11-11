using Developers.Models;
using Developers.Models.ViewModels;
using Developers.Repositories.Interfaces;
using Developers.Utilities;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using iText.Layout;
using iText.Layout.Element;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.IO;

namespace Developers.Controllers;
[Authorize(Roles = DS.Role_Admin + "," + DS.Role_Empleado)]
public class VentasController : Controller
{
    private readonly IUnitWork _unitWork;
    public VentasController(IUnitWork unitWork)
    {
        _unitWork = unitWork;
    }

    [HttpGet]
 
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet] //Mostrar el formulario de creación
    public IActionResult Create()
    {
        VentaVM ventaVM = new VentaVM()
        {
            Venta = new Models.Venta(),
            ProductoList = _unitWork.Venta.ObtenerTodosDropdownLista("Producto"),
        };
        return View(ventaVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VentaVM ventaVM)
    {
        if (ventaVM == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var claimIdentity = (ClaimsIdentity)this.User.Identity;
                var actualUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

                ventaVM.Venta.ApplicationUserId = actualUser.Value;

                // Parsear el JSON de los detalles de productos
                var productosDetalle = JsonConvert.DeserializeObject<List<VentaDetalle>>(ventaVM.ProductosDetalleJson);

                if (productosDetalle != null && productosDetalle.Any())
                {
                    // Inicializar la lista de detalles de la venta
                    ventaVM.Venta.VentaDetalles = new List<VentaDetalle>();

                    foreach (var detalle in productosDetalle)
                    {
                        var producto = await _unitWork.Producto.ObtenerPrimeroAsync(p => p.ProductoId == detalle.ProductoId, isTracking: true);

                        if (producto == null)
                        {
                            return NotFound();
                        }

                        if (producto.Cantidad >= detalle.Cantidad)
                        {
                            // Descontar la cantidad del producto
                            producto.Cantidad -= detalle.Cantidad;
                            _unitWork.Producto.Actualizar(producto);

                            // Agregar el detalle a la venta
                            detalle.Venta = ventaVM.Venta;
                            detalle.Producto = producto;  // Asegurarse de que el producto esté asignado
                            ventaVM.Venta.VentaDetalles.Add(detalle);
                        }
                        else
                        {
                            TempData[DS.Error] = $"No hay suficiente cantidad del producto {producto.ProductoName} para completar la venta.";
                            return View(ventaVM);
                        }
                    }

                    // Persistir la venta y los detalles
                    await _unitWork.Venta.AgregarAsync(ventaVM.Venta);
                    await _unitWork.GuardarAsync();

                    // Generar el PDF
                    var pdfBytes = GenerateVentaPDF(ventaVM.Venta);
                    TempData[DS.Successfull] = "Venta generada correctamente";

                    // Descargar el PDF
                    return File(pdfBytes, "application/pdf", $"Venta_{ventaVM.Venta.VentaId}.pdf");
                }
                else
                {
                    TempData[DS.Error] = "Debe agregar al menos un producto a la venta.";
                }
            }
            catch (Exception ex)
            {
                TempData[DS.Error] = $"Error al guardar la venta: {ex.Message}";
                Console.WriteLine($"Error al guardar la venta: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        ventaVM.ProductoList = _unitWork.Venta.ObtenerTodosDropdownLista("Producto");
        return View(ventaVM);
    }


    private byte[] GenerateVentaPDF(Venta venta)
    {
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Encabezado
                    document.Add(new Paragraph("FARMACIA LINARES")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFontSize(18)
                        .SetBold());
                    document.Add(new Paragraph("RUC 20202020201")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFontSize(14));
                    document.Add(new Paragraph("DIRECCIÓN: JIRON PROTUGAL 236 - CAJAMARCA, CAJAMARCA, CAJAMARCA")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFontSize(12));
                    document.Add(new Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy}")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFontSize(12));
                    document.Add(new Paragraph($"N00-{venta.VentaId.ToString("D8")}")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFontSize(12));
                    document.Add(new Paragraph("\n")); // Línea en blanco para separar el encabezado de la tabla

                    // Información de la venta
                    document.Add(new Paragraph($"Cliente: {venta.ClienteName}")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                        .SetFontSize(12));

                    // Tabla de productos
                    var table = new Table(4).UseAllAvailableWidth();
                    table.AddHeaderCell("Producto");
                    table.AddHeaderCell("Cantidad");
                    table.AddHeaderCell("Precio Unitario");
                    table.AddHeaderCell("Total");

                    foreach (var detalle in venta.VentaDetalles)
                    {
                        if (detalle.Producto == null)
                        {
                            throw new Exception("Producto es null en el detalle.");
                        }

                        table.AddCell(new Cell().Add(new Paragraph(detalle.Producto.ProductoName ?? "N/A")));
                        table.AddCell(new Cell().Add(new Paragraph(detalle.Cantidad.ToString())));
                        table.AddCell(new Cell().Add(new Paragraph(detalle.PrecioUnitario.ToString("C"))));
                        table.AddCell(new Cell().Add(new Paragraph((detalle.Cantidad * detalle.PrecioUnitario).ToString("C"))));
                    }

                    decimal totalVenta = venta.VentaDetalles.Sum(d => d.Cantidad * d.PrecioUnitario);
                    table.AddCell(new Cell(1, 3).Add(new Paragraph("Total Venta")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)));
                    table.AddCell(new Cell().Add(new Paragraph(totalVenta.ToString("C"))));

                    document.Add(table);
                    document.Close();
                }

                return memoryStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            // Manejo general de excepciones
            Console.WriteLine($"Error al generar el PDF: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }


    public IActionResult TestPDF()
    {
        var samplePdfBytes = GenerateSamplePDF();
        return File(samplePdfBytes, "application/pdf", "Sample.pdf");
    }

    public byte[] GenerateSamplePDF()
    {
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    document.Add(new Paragraph("Sample PDF")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFontSize(20));

                    document.Add(new Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy}"));
                    document.Add(new Paragraph("Cliente: Cliente de Prueba"));

                    var table = new Table(3).UseAllAvailableWidth();
                    table.AddHeaderCell("Columna 1");
                    table.AddHeaderCell("Columna 2");
                    table.AddHeaderCell("Columna 3");

                    table.AddCell("Dato 1");
                    table.AddCell("Dato 2");
                    table.AddCell("Dato 3");

                    document.Add(table);
                    document.Close();
                }

                return memoryStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar el PDF de prueba: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
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

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
            return NotFound();

        var producto = await _unitWork.Producto.ObtenerAsync(id.GetValueOrDefault());

        if (producto is null)
            return NotFound();

        return View(producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Producto producto)
    {

        if (ModelState.IsValid)
        {
            _unitWork.Producto.Actualizar(producto);
            await _unitWork.GuardarAsync();
            TempData["Successful"] = "Producto actualizado correctamente";
            return RedirectToAction("Index");
        }
        return View(producto);
    }

    [HttpGet]
    public async Task<IActionResult> SearchProduct(string term)
    {
        if (!string.IsNullOrEmpty(term))
        {
            var productList = await _unitWork.Producto.ObtenerTodosAsync(p => p.Status == true);
            var data = productList
                .Where(x => x.ProductoName.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(x => new { productoId = x.ProductoId, productoName = x.ProductoName })
                .ToList();
            return Json(data);
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
        var ventas = await _unitWork.Venta.ObtenerTodosAsync(
            includeProperties: "Producto",
            orderBy: c => c.OrderByDescending(c => c.VentaId),
            isTracking: false);

        return Json(new { data = ventas });
    }

    /// <summary>
    /// Eliminar un registro enviado por Ajax
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Json</returns>
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var ventasDB = await _unitWork.Venta.ObtenerAsync(id);

        if (ventasDB is null) // Si es nulo muestre un mensaje
            return Json(new { success = false, message = "Error al eliminar la venta" });

        // Si eliminó correctamente muestre mensaje
        _unitWork.Venta.Remover(ventasDB);
        await _unitWork.GuardarAsync();

        return Json(new { success = true, message = "Venta eliminada correctamente" });
    }
    #endregion


    
}
