namespace DocTask.Core.Models;

public partial class Unit
{
    public int UnitId { get; set; }

    public int OrgId { get; set; }

    public string UnitName { get; set; } = null!;

    public string? Type { get; set; }

    public int? UnitParent { get; set; }

    public virtual Org Org { get; set; } = null!;

    public virtual ICollection<Reminderunit> Reminderunits { get; set; } = new List<Reminderunit>();

    public virtual ICollection<Taskunitassignment> Taskunitassignments { get; set; } = new List<Taskunitassignment>();

    public virtual ICollection<Unituser> Unitusers { get; set; } = new List<Unituser>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
