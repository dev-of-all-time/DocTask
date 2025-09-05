namespace DocTask.Core.Models;

public partial class Userrole
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public int Roleid { get; set; }

    public DateTime Createdat { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
