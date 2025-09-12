namespace DocTask.Core.Models;

public partial class Unituser
{
    public int UnitUserId { get; set; }

    public int UnitId { get; set; }

    public int UserId { get; set; }

    public string? Position { get; set; }

    public int? Level { get; set; }

    public virtual Unit Unit { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
