namespace DocTask.Core.Models;

public partial class Reminderunit
{
    public int Id { get; set; }

    public int Reminderid { get; set; }

    public int Unitid { get; set; }

    public virtual Reminder Reminder { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
