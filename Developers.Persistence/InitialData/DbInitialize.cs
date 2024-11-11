using Developers.Models;
using Developers.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Developers.Persistence.InitialData;

public class DbInitialize : IDbInitialize
{
    private readonly DevelopersDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public DbInitialize(
        DevelopersDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public void Initialize()
    {
        // Ejecutar las migraciones pendientes
        try
        {
            if (_context.Database.GetPendingMigrations().Count() > 0)
            {
                _context.Database.Migrate(); //Ejecuta las migraciones pendientes
            }
        }
        catch (Exception)
        {
            throw;
        }

        // Datos Iniciales
        if (_context.Roles.Any()) return; // Si hay registros de Roles

        // Creamos los roles
        _roleManager.CreateAsync(new IdentityRole(DS.Role_Admin)).GetAwaiter().GetResult();
        _roleManager.CreateAsync(new IdentityRole(DS.Role_Empleado)).GetAwaiter().GetResult();

        // Crear al menos un usuario administrador
        _userManager.CreateAsync(new ApplicationUser
        {
            UserName = "admin@correo.com",
            Email = "admin@correo.com",
            EmailConfirmed = true,
            FirstName = "Estefany",
            LastName = "Saldaña",
            PhoneNumber= "123456789",
        }, "Admin123*").GetAwaiter().GetResult();

        // Asignar el rol de administrador
        ApplicationUser user = _context.ApplicationUser.Where(u => u.UserName == "admin@correo.com").FirstOrDefault();
        _userManager.AddToRoleAsync(user, DS.Role_Admin).GetAwaiter().GetResult();
    }
}
