using Developers.Models;
using Developers.Persistence;
using Developers.Repositories.Interfaces;
using Developers.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Developers.Controllers;

[Authorize(Roles = DS.Role_Admin)]

public class UsersController : Controller
{
    private readonly IUnitWork _unitWork;
    private readonly DevelopersDbContext _context;
    public UsersController(IUnitWork unitWork, DevelopersDbContext context)
    {
        _unitWork = unitWork;
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var user = await _unitWork.ApplicationUser.ObtenerPrimeroAsync(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.Id);
        if (userRole != null)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == userRole.RoleId);
            if (role != null)
            {
                user.Role = role.Name;
            }
        }

        var roles = await _context.Roles.ToListAsync();
        ViewBag.Roles = new SelectList(roles, "Name", "Name");

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ApplicationUser user)
    {
        if (ModelState.IsValid)
        {
            var userInDb = await _unitWork.ApplicationUser.ObtenerPrimeroAsync(u => u.Id == user.Id);
            if (userInDb == null)
            {
                return NotFound();
            }

            userInDb.FirstName = user.FirstName;
            userInDb.LastName = user.LastName;
            userInDb.Email = user.Email;
            userInDb.PhoneNumber = user.PhoneNumber;

            // Obtener el rol seleccionado por su nombre
            var selectedRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == user.Role);
            if (selectedRole != null)
            {
                // Eliminar la relación actual entre usuario y roles
                var userRoles = await _context.UserRoles.Where(ur => ur.UserId == userInDb.Id).ToListAsync();
                _context.UserRoles.RemoveRange(userRoles);

                // Crear una nueva relación entre usuario y rol
                var newUserRole = new IdentityUserRole<string> { UserId = userInDb.Id, RoleId = selectedRole.Id };
                _context.UserRoles.Add(newUserRole);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
                TempData[DS.Successfull] = "Usuario editado correctamente";

                return RedirectToAction(nameof(Index));
            }
        }

        // Si hay errores de validación, recargar los roles y mostrar el formulario de nuevo
        var roles = await _context.Roles.ToListAsync();
        ViewBag.Roles = new SelectList(roles, "Name", "Name", user.Role);
        TempData[DS.Error] = "Error al editar usuario";

        return View(user);
    }


    #region API para JAVASCRIPT
    /// <summary>
    /// Lista todos los usuarios existentes
    /// </summary>
    /// <returns>Json</returns>
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        // Tdos los usuarios
        //var users = await _unitWork.ApplicationUser.ObtenerTodosAsync();

        // Todos los usuarios, excepto el que tiene la sesión activa  
        var claimIdentity = (ClaimsIdentity)this.User.Identity; // Usuario logueado
        var actualUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier); // Name usuario logueado (correo)
        var users = await _unitWork.ApplicationUser.ObtenerTodosAsync(filter: u => u.Id != actualUser.Value);

        // Los roles
        var userRoles = await _context.UserRoles.ToListAsync();
        var roles = await _context.Roles.ToListAsync();

        // Llenar la propiedad Role del Modelo ApplicationUser
        foreach (var user in users)
        {
            var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
            user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
        }

        return Json(new { data = users });
    }
    #endregion
}
