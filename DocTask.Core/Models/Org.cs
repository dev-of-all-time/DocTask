namespace DocTask.Core.Models;

public partial class Org
{
    public int OrgId { get; set; }

    public string OrgName { get; set; } = null!;

    public int? ParentOrgId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
