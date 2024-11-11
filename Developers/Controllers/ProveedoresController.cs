using Developers.Models;
using Developers.Repositories.Interfaces;
using Developers.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Developers.Controllers;
[Authorize(Roles = DS.Role_Admin + "," + DS.Role_Empleado)]

public class ProveedoresController : Controller
{
    private readonly IUnitWork _unitWork;
    public ProveedoresController(IUnitWork unitWork)
    {
        _unitWork = unitWork;
    }


    [HttpGet]

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        Proveedor proveedor = new Proveedor();
        return View(proveedor);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Proveedor proveedor)
    {
        // Verificar si el nombre del proveedor ya existe (insensible a mayúsculas)
        var proveedorExistente = await _unitWork.Proveedor
            .ObtenerPrimeroAsync(p => p.ProveedorName.ToLower() == proveedor.ProveedorName.ToLower());

        if (proveedorExistente != null)
        {
            // Usar TempData para mensajes Toastr
            TempData[DS.Error] = "El nombre del proveedor ya está en uso, por favor elija otro.";
            return View(proveedor);
        }

        if (ModelState.IsValid)
        {
            proveedor.CreatedAt = DateTime.Now;
            proveedor.UpdatedAt = DateTime.Now;
            proveedor.Status = true;

            await _unitWork.Proveedor.AgregarAsync(proveedor);
            await _unitWork.GuardarAsync();

            TempData[DS.Successfull] = "Proveedor creado correctamente";
            return RedirectToAction("Index");
        }

        // Si el modelo no es válido, devolver la vista con el modelo
        return View(proveedor);
    }



    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        Proveedor proveedor = new Proveedor();
        proveedor = await _unitWork.Proveedor.ObtenerAsync(id.GetValueOrDefault());

        if (proveedor is null) return NotFound();

        return View(proveedor);

    }

    [HttpPost]
    public async Task<IActionResult> Edit(Proveedor proveedor)
    {
        if (ModelState.IsValid)
        {
            _unitWork.Proveedor.Actualizar(proveedor);
            await _unitWork.GuardarAsync();

            TempData["Successful"] = "Proveedor actualizado correctamente";

            return RedirectToAction("Index");
        }

        return View(proveedor);
    }

    [HttpGet]
    public async Task<IActionResult> Vista(int? id)
    {
        if (id is null)
            return NotFound();

        //var trainer = await _dbContext.Trainers.FindAsync(id);
        var proveedor = await _unitWork.Proveedor.ObtenerAsync(id.GetValueOrDefault());

        if (proveedor is null)
            return NotFound();

        return View(proveedor);
    }

    #region API
    /// <summary>
    /// Listar todos los proveedores registrados
    /// </summary>
    /// <returns>Json</returns>
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var proveedores = await _unitWork.Proveedor.ObtenerTodosAsync(
            orderBy: c => c.OrderByDescending(c => c.ProveedorId),
            isTracking: false);

        return Json(new { data = proveedores });
    }

    /// <summary>
    /// Eliminar un registro enviado por Ajax
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Json</returns>
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var proveedoresDB = await _unitWork.Proveedor.ObtenerAsync(id);

        if (proveedoresDB is null) // Si es nulo muestre un mensaje
            return Json(new { success = false, message = "Error al eliminar el proveedor" });

        // Si eliminó correctamente muestre mensaje
        _unitWork.Proveedor.Remover(proveedoresDB);
        await _unitWork.GuardarAsync();

        return Json(new { success = true, message = "Proveedor eliminado correctamente" });
    }
    #endregion
}

