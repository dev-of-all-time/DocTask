namespace DocTask.Core.Models;

public partial class Reminder
{
    public int Reminderid { get; set; }

    public int Taskid { get; set; }

    public int? Periodid { get; set; }

    public string Message { get; set; } = null!;

    public DateTime Triggertime { get; set; }

    public bool? Isauto { get; set; }

    public int? Createdby { get; set; }

    public DateTime Createdat { get; set; }

    public bool? Isnotified { get; set; }

    public DateTime? Notifiedat { get; set; }

    public int? Notificationid { get; set; }

    public string Title { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual User? CreatedbyNavigation { get; set; }

    public virtual Notification? Notification { get; set; }

    public virtual Period? Period { get; set; }

    public virtual ICollection<Reminderunit> Reminderunits { get; set; } = new List<Reminderunit>();

    public virtual Task Task { get; set; } = null!;

    public virtual User? User { get; set; }
}
