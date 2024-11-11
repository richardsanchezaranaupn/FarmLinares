namespace Developers.Models;

public class EntityBase
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Status { get; set; }

    public EntityBase()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        Status = true;
    }
}
