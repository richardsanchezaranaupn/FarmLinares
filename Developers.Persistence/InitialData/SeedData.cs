using Developers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Developers.Persistence.InitialData;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new DevelopersDbContext(serviceProvider.GetRequiredService<DbContextOptions<DevelopersDbContext>>()))
        {

            //Proyecto------------------------------------------------------


            // CATEGORIAS

            if (context.Categorias.Any()) return;
            var categorias = new Categoria[]
            {
            new Categoria { CategoriaName = "Medicamentos de Prescripción", Descripcion = "Medicamentos que requieren receta médica" },
            new Categoria { CategoriaName = "Medicamentos de Venta Libre", Descripcion = "Medicamentos que no requieren receta médica" },
            new Categoria { CategoriaName = "Suplementos y Vitaminas", Descripcion = "Suplementos dietéticos y vitaminas para diversas necesidades de salud" },
            new Categoria { CategoriaName = "Cuidado de la Piel", Descripcion = "Productos de cuidado de la piel, incluyendo cremas, limpiadores y sueros" },
            new Categoria { CategoriaName = "Higiene Personal", Descripcion = "Productos de higiene personal como champús, jabones y desodorantes" },
            new Categoria { CategoriaName = "Cuidado del Bebé", Descripcion = "Productos para el cuidado del bebé, incluyendo pañales y toallitas húmedas" },
            new Categoria { CategoriaName = "Salud Femenina", Descripcion = "Productos para la salud femenina, incluyendo productos menstruales y de maternidad" },
            new Categoria { CategoriaName = "Productos Naturales", Descripcion = "Productos naturales y orgánicos" },
            new Categoria { CategoriaName = "Equipos Médicos", Descripcion = "Equipos y dispositivos médicos" },
            new Categoria { CategoriaName = "Nutrición y Dieta", Descripcion = "Productos de nutrición y dieta, incluyendo alimentos saludables y productos para el control de peso" }
            };

            foreach (var categoria in categorias)
            {
                context.Categorias.Add(categoria);
            }
            context.SaveChanges();

            // PROVEEDORES

            if (context.Proveedores.Any()) return;
            var proveedores = new Proveedor[]
            {
            new Proveedor { ProveedorName = "Laboratorios Roche", Ruc = "01234567891", Direccion = "Av. San Luis 1234, Lima, Perú", NombreContacto = "Juan Pérez", Telefono = "987654321", CorreoElectronico = "contacto@roche.com" },
            new Proveedor { ProveedorName = "Bayer S.A.", Ruc = "09876543211", Direccion = "Calle Los Alamos 456, Santiago, Chile", NombreContacto = "María Gómez", Telefono = "123456789", CorreoElectronico = "contacto@bayer.com" },
            new Proveedor { ProveedorName = "Johnson & Johnson", Ruc = "01928374651", Direccion = "Av. Principal 789, Buenos Aires, Argentina", NombreContacto = "Carlos Fernández", Telefono = "234567890", CorreoElectronico = "contacto@jnj.com" },
            new Proveedor { ProveedorName = "Pfizer Inc.", Ruc = "01029384751", Direccion = "Calle Central 102, Bogotá, Colombia", NombreContacto = "Ana Ramírez", Telefono = "345678901", CorreoElectronico = "contacto@pfizer.com" },
            new Proveedor { ProveedorName = "Nestlé Health Science", Ruc = "05647382911", Direccion = "Av. Secundaria 321, Quito, Ecuador", NombreContacto = "Luis Herrera", Telefono = "456789012", CorreoElectronico = "contacto@nestle.com" },
            new Proveedor { ProveedorName = "Sanofi", Ruc = "02345678901", Direccion = "Calle Mayor 654, Montevideo, Uruguay", NombreContacto = "Sofía Núñez", Telefono = "567890123", CorreoElectronico = "contacto@sanofi.com" },
            new Proveedor { ProveedorName = "GlaxoSmithKline", Ruc = "03456789011", Direccion = "Av. Libertad 987, Ciudad de México, México", NombreContacto = "Fernando López", Telefono = "678901234", CorreoElectronico = "contacto@gsk.com" },
            new Proveedor { ProveedorName = "Merck & Co.", Ruc = "04567890121", Direccion = "Calle Nueva 321, Lima, Perú", NombreContacto = "Gabriela Torres", Telefono = "789012345", CorreoElectronico = "contacto@merck.com" },
            new Proveedor { ProveedorName = "AbbVie Inc.", Ruc = "05678901231", Direccion = "Av. Independencia 654, Caracas, Venezuela", NombreContacto = "Ricardo Martínez", Telefono = "890123456", CorreoElectronico = "contacto@abbvie.com" },
            new Proveedor { ProveedorName = "Novartis", Ruc = "06789012341", Direccion = "Calle Antigua 789, San José, Costa Rica", NombreContacto = "Carmen Ruiz", Telefono = "901234567", CorreoElectronico = "contacto@novartis.com" }
                    };

            foreach (var proveedor in proveedores)
            {
                context.Proveedores.Add(proveedor);
            }
            context.SaveChanges();




        }
    }
}
