using Developers.Models;
using Developers.Repositories.Interfaces;
using Developers.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Developers.Controllers;
[Authorize(Roles = DS.Role_Admin + "," + DS.Role_Empleado)]
public class CategoriasController : Controller
{
    private readonly IUnitWork _unitWork;
    public CategoriasController(IUnitWork unitWork)
    {
        _unitWork = unitWork;
    }


    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create() {
        Categoria categoria = new Categoria();
        return View(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Categoria categoria)
    {
        if (categoria == null)
        {
            TempData[DS.Error] = "La categoría no puede estar vacía.";
            return View(categoria);
        }

        // Verificar si el nombre de la categoría ya existe (insensible a mayúsculas)
        var categoriaExistente = await _unitWork.Categoria
            .ObtenerPrimeroAsync(c => c.CategoriaName.ToLower() == categoria.CategoriaName.ToLower());

        if (categoriaExistente != null)
        {
            // Usar TempData para mensajes Toastr
            TempData[DS.Error] = "El nombre de la categoría ya está en uso, por favor elija otro.";
            return View(categoria);
        }

        if (ModelState.IsValid)
        {
            categoria.CreatedAt = DateTime.Now;
            categoria.UpdatedAt = DateTime.Now;
            categoria.Status = true;

            await _unitWork.Categoria.AgregarAsync(categoria);
            await _unitWork.GuardarAsync();

            TempData[DS.Successfull] = "Categoría creada correctamente";
            return RedirectToAction("Index");
        }

        // Si el modelo no es válido, devolver la vista con el modelo
        TempData[DS.Error] = "Error al crear la categoría. Por favor, verifica los datos ingresados.";
        return View(categoria);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        Categoria categoria = new Categoria();
        categoria = await _unitWork.Categoria.ObtenerAsync(id.GetValueOrDefault());

        if (categoria is null) return NotFound(); 

        return View(categoria);

    }

    [HttpPost]
    public async Task<IActionResult> Edit(Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            _unitWork.Categoria.Actualizar(categoria);
            await _unitWork.GuardarAsync();

            TempData["Successful"] = "Categoria actualizada correctamente";

            return RedirectToAction("Index");
        }

        return View(categoria);
    }

    [HttpGet]
    public async Task<IActionResult> Vista(int? id)
    {
        if (id is null)
            return NotFound();

        //var trainer = await _dbContext.Trainers.FindAsync(id);
        var categoria = await _unitWork.Categoria.ObtenerAsync(id.GetValueOrDefault());

        if (categoria is null)
            return NotFound();

        return View(categoria);
    }

    #region API
    /// <summary>
    /// Listar todos los cursos registrados
    /// </summary>
    /// <returns>Json</returns>
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var categorias = await _unitWork.Categoria.ObtenerTodosAsync(
            orderBy: c => c.OrderByDescending(c => c.CategoriaId),
            isTracking: false);

        return Json(new { data = categorias });
    }

    /// <summary>
    /// Eliminar un registro enviado por Ajax
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Json</returns>
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var categoriasDB = await _unitWork.Categoria.ObtenerAsync(id);
        
        if (categoriasDB is null) // Si es nulo muestre un mensaje
            return Json(new { success = false, message="Error al eliminar la categoria" });

        // Si eliminó correctamente muestre mensaje
        _unitWork.Categoria.Remover(categoriasDB);
        await _unitWork.GuardarAsync();

        return Json(new { success = true, message = "Categoria eliminada correctamente" });    
    }
    #endregion
}
