using Developers.Models;
using NUnit.Framework.Internal.Execution;
using System.ComponentModel.DataAnnotations;

namespace PruebasUnitarias
{
    [TestFixture]
    public class ProveedorTests
    {

        [Test]
        public void ValidacionDatosProveedorConRucString_Valido()
        {
            // Datos de prueba
            string ruc = "26015231235";  // RUC v�lido con 11 d�gitos

            Proveedor p = new Proveedor
            {
                ProveedorName = "Demo",
                Ruc = ruc
            };

            // Variable bool para el resultado de validaci�n
            bool esRucValido = false;

            // Verificar que Ruc es un string
            if (p.Ruc is string && p.Ruc.Length == 11 && p.Ruc.All(char.IsDigit))
            {
                esRucValido = true;
            }

            // Verificar que Ruc es un string
            Assert.IsInstanceOf<string>(p.Ruc, "El RUC debe ser de tipo string.");

            // Verificar la longitud del RUC
            Assert.AreEqual(11, p.Ruc.Length, "La longitud del RUC debe ser de 11 caracteres.");

            // Verificar que todos los caracteres del RUC son d�gitos
            Assert.IsTrue(p.Ruc.All(char.IsDigit), "El RUC debe contener solo d�gitos.");

            // Verificar el valor final del bool para mostrar resultado positivo o negativo
            Assert.IsTrue(esRucValido, "El RUC es v�lido y cumple con los criterios de longitud y formato.");
        }

        [Test]
        public void ValidacionDatosProveedorConRucString_Invalido()
        {
            // Datos de prueba inv�lidos
            string ruc = "26A15231235";  // RUC con un car�cter no num�rico

            Proveedor p = new Proveedor
            {
                ProveedorName = "Demo",
                Ruc = ruc
            };

            // Variable bool para el resultado de validaci�n
            bool esRucValido = false;

            // Verificar que Ruc es un string
            if (p.Ruc is string && p.Ruc.Length == 11 && p.Ruc.All(char.IsDigit))
            {
                esRucValido = true;
            }

            // Verificar que Ruc es un string
            Assert.IsInstanceOf<string>(p.Ruc, "El RUC debe ser de tipo string.");

            // Verificar la longitud del RUC
            Assert.AreEqual(11, p.Ruc.Length, "La longitud del RUC debe ser de 11 caracteres.");

            // Verificar que todos los caracteres del RUC son d�gitos
            Assert.IsFalse(p.Ruc.All(char.IsDigit), "El RUC contiene caracteres no num�ricos.");

            // Verificar el valor final del bool para mostrar resultado negativo
            Assert.IsFalse(esRucValido, "El RUC no es v�lido debido a caracteres no num�ricos.");
        }

        [Test]
        public void ValidacionNombreProveedor_MenorA200Caracteres()
        {
            // Datos de prueba: nombre v�lido con menos de 200 caracteres
            string nombreValido = new string('A', 199);  // Un string de 199 caracteres

            Proveedor p = new Proveedor
            {
                ProveedorName = nombreValido
            };

            // Verificar que el nombre es de tipo string
            Assert.IsInstanceOf<string>(p.ProveedorName, "El nombre del proveedor debe ser de tipo string.");

            // Verificar que la longitud del nombre sea menor o igual a 200 caracteres
            Assert.LessOrEqual(p.ProveedorName.Length, 200, "El nombre del proveedor no debe " +
                "exceder los 200 caracteres.");
        }

        [Test]
        public void ValidacionNombreContacto_Requerido()
        {
            var proveedor = new Proveedor { NombreContacto = null };
            var context = new ValidationContext(proveedor);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(proveedor, context, results, true);

            Assert.IsFalse(esValido, "El nombre de contacto es obligatorio.");
        }

        [Test]
        public void ValidacionTelefono_Invalido()
        {
            var proveedor = new Proveedor { Telefono = "noesnumero" };
            var context = new ValidationContext(proveedor);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(proveedor, context, results, true);

            Assert.IsFalse(esValido, "El n�mero de tel�fono no es v�lido.");
        }

        [Test]
        public void ValidacionCorreoElectronico_Invalido()
        {
            var proveedor = new Proveedor { CorreoElectronico = "noescorreo" };
            var context = new ValidationContext(proveedor);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(proveedor, context, results, true);

            Assert.IsFalse(esValido, "El formato del correo electr�nico es incorrecto.");
        }

        [Test]
        public void ValidacionNombreProveedor_Obligatorio()
        {
            var proveedor = new Proveedor { ProveedorName = null }; // Nombre del proveedor es nulo
            var context = new ValidationContext(proveedor);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(proveedor, context, results, true);

            Assert.IsFalse(esValido, "El nombre del proveedor es obligatorio.");
        }

        [Test]
        public void ValidacionTelefono_LargoExcedido()
        {
            var proveedor = new Proveedor
            {
                Telefono = new string('1', 16), // Tel�fono con m�s de 15 caracteres
                ProveedorName = "Proveedor de Prueba",
                Ruc = "26015231235",
                NombreContacto = "Contacto Prueba",
                CorreoElectronico = "contacto@proveedor.com"
            };
            var context = new ValidationContext(proveedor);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(proveedor, context, results, true);

            Assert.IsFalse(esValido, "El n�mero de tel�fono no debe exceder los 15 caracteres.");
        }

        [Test]
        public void ValidacionCorreoElectronico_LongitudExcedida()
        {
            var proveedor = new Proveedor
            {
                CorreoElectronico = new string('a', 101) + "@test.com", // Correo con m�s de 100 caracteres
                ProveedorName = "Proveedor de Prueba",
                Ruc = "26015231235",
                NombreContacto = "Contacto Prueba",
                Telefono = "1234567890"
            };
            var context = new ValidationContext(proveedor);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(proveedor, context, results, true);

            Assert.IsFalse(esValido, "El correo electr�nico no debe exceder los 100 caracteres.");
        }

        [Test]
        public void ValidacionRuc_NoPuedeSerNulo()
        {
            var proveedor = new Proveedor { Ruc = null }; // RUC nulo
            var context = new ValidationContext(proveedor);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(proveedor, context, results, true);

            Assert.IsFalse(esValido, "El RUC es obligatorio y no puede ser nulo.");
        }

    }

    [TestFixture]
    public class VentaTests
    {

        [Test]
        public void ValidacionDatosVentaConClienteName_Valido()
        {
            // Datos de prueba
            string clienteName = "Cliente de Prueba";  // ClienteName v�lido con menos de 200 caracteres

            Venta v = new Venta
            {
                ClienteName = clienteName
            };

            // Verificar que ClienteName es un string
            Assert.IsInstanceOf<string>(v.ClienteName, "El nombre del cliente debe ser de tipo string.");

            // Verificar que ClienteName no es nulo o vac�o
            Assert.IsFalse(string.IsNullOrEmpty(v.ClienteName), "El nombre del cliente es obligatorio y no puede ser nulo o vac�o.");

            // Verificar la longitud de ClienteName
            Assert.LessOrEqual(v.ClienteName.Length, 200, "El nombre del cliente debe tener como m�ximo 200 caracteres.");
        }

        [Test]
        public void ValidacionDatosVentaConClienteName_Invalido()
        {
            // Datos de prueba inv�lidos: longitud excede los 200 caracteres
            string clienteName = new string('A', 201);  // ClienteName con m�s de 200 caracteres

            Venta v = new Venta
            {
                ClienteName = clienteName
            };

            // Verificar que ClienteName es un string
            Assert.IsInstanceOf<string>(v.ClienteName, "El nombre del cliente debe ser de tipo string.");

            // Verificar que ClienteName no es nulo o vac�o
            Assert.IsFalse(string.IsNullOrEmpty(v.ClienteName), "El nombre del cliente es obligatorio y no puede ser nulo o vac�o.");

            // Verificar que la longitud de ClienteName excede el m�ximo permitido
            Assert.Greater(v.ClienteName.Length, 200, "El nombre del cliente no debe exceder los 200 caracteres.");
        }

        [Test]
        public void ValidacionDatosVentaConClienteName_Nulo()
        {
            // Datos de prueba inv�lidos: ClienteName es nulo o vac�o
            string clienteName = null;  // ClienteName nulo

            Venta v = new Venta
            {
                ClienteName = clienteName
            };

            // Verificar que ClienteName es nulo o vac�o
            Assert.IsTrue(string.IsNullOrEmpty(v.ClienteName), "El nombre del cliente no debe ser nulo o vac�o.");
        }

        [Test]
        public void ValidacionClienteName_Requerido()
        {
            var venta = new Venta { ClienteName = null };
            var context = new ValidationContext(venta);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(venta, context, results, true);

            Assert.IsFalse(esValido, "El nombre del cliente es obligatorio.");
        }

        [Test]
        public void ValidacionClienteName_Largo()
        {
            var venta = new Venta { ClienteName = new string('A', 201) };
            var context = new ValidationContext(venta);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(venta, context, results, true);

            Assert.IsFalse(esValido, "El nombre del cliente no debe exceder los 200 caracteres.");
        }

        [Test]
        public void ValidacionFechaRegistro_Obligatorio()
        {
            var venta = new Venta
            {
                FechaRegistro = default // Fecha no establecida
            };
            var context = new ValidationContext(venta);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(venta, context, results, true);

            Assert.IsFalse(esValido, "La fecha de registro es obligatoria.");
        }

        [Test]
        public void ValidacionClienteName_NoPuedeSerSoloEspacios()
        {
            var venta = new Venta
            {
                ClienteName = "     " // Solo espacios en blanco
            };
            var context = new ValidationContext(venta);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(venta, context, results, true);

            Assert.IsFalse(esValido, "El nombre del cliente no puede estar vac�o o contener solo espacios en blanco.");
        }


    }

    [TestFixture]
    public class CategoriaTests
    {
        [Test]
        public void ValidacionCategoriaNameRequerido()
        {
            // Datos de prueba
            Categoria categoriaSinNombre = new Categoria
            {
                CategoriaName = "",  // Nombre vac�o
                Descripcion = "Descripci�n de prueba"
            };

            // Validaci�n del resultado
            bool esValido = !string.IsNullOrEmpty(categoriaSinNombre.CategoriaName);

            // La prueba debe fallar si el nombre est� vac�o
            Assert.IsFalse(esValido, "El nombre de la categor�a es obligatorio.");
        }

        [Test]
        public void ValidacionCategoriaNameValido()
        {
            // Datos de prueba con nombre v�lido
            Categoria categoriaConNombre = new Categoria
            {
                CategoriaName = "Tecnolog�a",  // Nombre v�lido
                Descripcion = "Categor�a de productos tecnol�gicos"
            };

            // Validaci�n del resultado
            bool esValido = !string.IsNullOrEmpty(categoriaConNombre.CategoriaName);

            // La prueba debe pasar si el nombre no est� vac�o
            Assert.IsTrue(esValido, "El nombre de la categor�a es v�lido y cumple los requisitos.");
        }


        [Test]
        public void ValidacionCategoriaName_Requerido()
        {
            var categoria = new Categoria { CategoriaName = null };
            var context = new ValidationContext(categoria);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(categoria, context, results, true);

            Assert.IsFalse(esValido, "El nombre de la categor�a es obligatorio.");
        }

        [Test]
        public void ValidacionCategoriaDescripcion_Valido()
        {
            var categoria = new Categoria { CategoriaName = "Electr�nica", Descripcion = "Descripci�n v�lida" };
            var context = new ValidationContext(categoria);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(categoria, context, results, true);

            Assert.IsTrue(esValido, "La categor�a es v�lida.");
        }

        [Test]
        public void ValidacionCategoriaName_NoPuedeSerSoloEspacios()
        {
            var categoria = new Categoria
            {
                CategoriaName = "    ", // Solo espacios en blanco
                Descripcion = "Descripci�n de prueba"
            };
            var context = new ValidationContext(categoria);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(categoria, context, results, true);

            Assert.IsFalse(esValido, "El nombre de la categor�a no puede estar vac�o o contener solo espacios en blanco.");
        }


        [Test]
        public void ValidacionCategoriaDescripcion_PuedeSerNula()
        {
            var categoria = new Categoria
            {
                CategoriaName = "Electr�nica",
                Descripcion = null // Descripci�n nula
            };
            var context = new ValidationContext(categoria);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(categoria, context, results, true);

            Assert.IsTrue(esValido, "La categor�a debe ser v�lida incluso si la descripci�n es nula.");
        }

        [Test]
        public void ValidacionCategoriaName_Unico()
        {
            var categoriasExistentes = new List<Categoria>
        {
            new Categoria { CategoriaName = "Tecnolog�a" },
            new Categoria { CategoriaName = "Moda" }
        };

            var nuevaCategoria = new Categoria { CategoriaName = "Tecnolog�a" }; // Nombre ya existente

            // Validar que la nueva categor�a no existe en la lista
            bool esUnico = !categoriasExistentes.Any(c => c.CategoriaName == nuevaCategoria.CategoriaName);

            Assert.IsFalse(esUnico, "El nombre de la categor�a debe ser �nico.");
        }

    }

    [TestFixture]
    public class ProductoTests
    {
        [Test]
        public void ValidacionProductoName_Requerido()
        {
            var producto = new Producto { ProductoName = null };
            var context = new ValidationContext(producto);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(producto, context, results, true);

            Assert.IsFalse(esValido, "El nombre del producto es obligatorio.");
        }


        [Test]
        public void ValidacionProductoPrecio_Invalido()
        {
            var producto = new Producto { Precio = -5.00m };
            var context = new ValidationContext(producto);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(producto, context, results, true);

            Assert.IsFalse(esValido, "El precio debe ser mayor que 0.");
        }

        [Test]
        public void ValidacionReceta_Requerida()
        {
            var producto = new Producto { Receta = null };
            var context = new ValidationContext(producto);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(producto, context, results, true);

            Assert.IsFalse(esValido, "El campo Receta es obligatorio.");
        }


        [Test]
        public void ValidacionProductoPrecio_Cero()
        {
            var producto = new Producto { Precio = 0.00m, ProductoName = "Producto de Prueba", Receta = "Receta de Prueba" };
            var context = new ValidationContext(producto);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(producto, context, results, true);

            Assert.IsFalse(esValido, "El precio no debe ser cero.");
        }

        [Test]
        public void ValidacionProductoPrecio_ExcedeLimiteSuperior()
        {
            var producto = new Producto { Precio = 100000.00m, ProductoName = "Producto de Prueba", Receta = "Receta de Prueba" };
            var context = new ValidationContext(producto);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(producto, context, results, true);

            Assert.IsFalse(esValido, "El precio no debe exceder 99999.99.");
        }

        [Test]
        public void ValidacionProductoName_EspaciosEnBlanco()
        {
            var producto = new Producto
            {
                ProductoName = "   ", // Nombre con solo espacios en blanco
                Receta = "Receta de Prueba",
                Precio = 10.00m
            };
            var context = new ValidationContext(producto);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(producto, context, results, true);

            Assert.IsFalse(esValido, "El nombre del producto no debe estar vac�o o contener solo espacios en blanco.");
        }

        [Test]
        public void ValidacionProductoPrecio_DecimalesExcesivos()
        {
            var producto = new Producto { Precio = 10.123m, ProductoName = "Producto de Prueba", Receta = "Receta de Prueba" };
            var context = new ValidationContext(producto);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(producto, context, results, true);

            Assert.IsFalse(esValido, "El precio no debe tener m�s de dos decimales.");
        }

        [Test]
        public void ValidacionProductoPrecio_MenorQueMinimo()
        {
            var producto = new Producto
            {
                Precio = 0.00m, // Precio en el l�mite inferior
                ProductoName = "Producto de Prueba",
                Receta = "Receta de Prueba"
            };
            var context = new ValidationContext(producto);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(producto, context, results, true);

            Assert.IsFalse(esValido, "El precio debe ser mayor que 0.01.");
        }

        [Test]
        public void ValidacionProductoName_NoPuedeSerVacio()
        {
            var producto = new Producto
            {
                ProductoName = string.Empty, // Nombre vac�o
                Receta = "Receta de Prueba",
                Precio = 10.00m
            };
            var context = new ValidationContext(producto);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(producto, context, results, true);

            Assert.IsFalse(esValido, "El nombre del producto no puede estar vac�o.");
        }

        [Test]
        public void ValidacionReceta_NoPuedeSerVacia()
        {
            var producto = new Producto
            {
                Receta = string.Empty, // Receta vac�a
                ProductoName = "Producto de Prueba",
                Precio = 10.00m
            };
            var context = new ValidationContext(producto);
            var results = new List<ValidationResult>();

            bool esValido = Validator.TryValidateObject(producto, context, results, true);

            Assert.IsFalse(esValido, "La receta no puede estar vac�a.");
        }

    }

}