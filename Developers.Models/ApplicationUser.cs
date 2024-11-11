using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Developers.Models;

public class ApplicationUser :IdentityUser
{
    [Required(ErrorMessage = "Nombres es requerido")]
    [MaxLength(100)]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Apellidos es requerido")]
    [MaxLength(100)]
    public string LastName { get; set; }
    [NotMapped] // Existe en el modelo pero no en la DB
    public string Role { get; set; }
}
