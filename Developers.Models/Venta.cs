using System.ComponentModel.DataAnnotations;

namespace Developers.Models;

public class Venta : EntityBase
{
    public int VentaId { get; set; }
    public DateTime FechaRegistro { get; set; }

    [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
    [StringLength(200, ErrorMessage = "El nombre del cliente no debe exceder los 200 caracteres.")]
    public string ClienteName { get; set; }

    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

    public ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}

