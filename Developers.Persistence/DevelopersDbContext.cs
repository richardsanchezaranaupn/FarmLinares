using Developers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal; 
using System.Reflection;

namespace Developers.Persistence;

//public class DevelopersDbContext : DbContext
public class DevelopersDbContext : IdentityDbContext
{
    public DevelopersDbContext(DbContextOptions<DevelopersDbContext> options) : base(options)
    { }
       
    //------------------------------------------------------------------------------>
    public DbSet<Categoria> Categorias { get; set; } //PROYECTO
    public DbSet<Proveedor> Proveedores { get; set; } //PROYECTO
    public DbSet<Producto> Productos { get; set; } //PROYECTO
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<Venta> Ventas { get; set; } //PROYECTO
    public DbSet<VentaDetalle> VentaDetalles { get; set; } //PROYECTO


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // La configuración del modelo esta en un archivo externo
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
