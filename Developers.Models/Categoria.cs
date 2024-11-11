using System.ComponentModel.DataAnnotations;

namespace Developers.Models;

public class Categoria : EntityBase
{
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    public string CategoriaName { get; set; }
    public string Descripcion { get; set; }
}

