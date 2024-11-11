using System.ComponentModel.DataAnnotations;

namespace Developers.Models;

public class Proveedor : EntityBase
{
    public int ProveedorId { get; set; }

    [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
    [StringLength(200, ErrorMessage = "El nombre del proveedor no debe exceder los 200 caracteres.")]
    public string ProveedorName { get; set; }

    [Required(ErrorMessage = "El RUC es obligatorio.")]
    [StringLength(11, ErrorMessage = "El RUC debe tener exactamente 11 caracteres.")]
    [RegularExpression(@"\d{11}", ErrorMessage = "El RUC debe contener exactamente 11 dígitos.")]
    public string Ruc { get; set; }

    public string Direccion { get; set; }

    [Required(ErrorMessage = "El nombre de contacto es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre de contacto no debe exceder los 150 caracteres.")]
    public string NombreContacto { get; set; }

    [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
    [Phone(ErrorMessage = "El número de teléfono no es válido.")]
    [StringLength(15, ErrorMessage = "El número de teléfono no debe exceder los 15 caracteres.")]
    public string Telefono { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico es incorrecto.")]
    [StringLength(100, ErrorMessage = "El correo electrónico no debe exceder los 100 caracteres.")]
    public string CorreoElectronico { get; set; }
}
    