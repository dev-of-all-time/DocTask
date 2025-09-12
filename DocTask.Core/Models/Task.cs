namespace DocTask.Core.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? AssignerId { get; set; }

    public int? AssigneeId { get; set; }

    public int? OrgId { get; set; }

    public int? PeriodId { get; set; }

    public int? AttachedFile { get; set; }

    public string? Status { get; set; }

    public string? Priority { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UnitId { get; set; }

    public int? FrequencyId { get; set; }

    public int? Percentagecomplete { get; set; }

    public int? ParentTaskId { get; set; }

    public virtual User? Assignee { get; set; }

    public virtual Frequency? Frequency { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    public virtual ICollection<Reportsummary> Reportsummaries { get; set; } = new List<Reportsummary>();

    public virtual ICollection<Taskunitassignment> Taskunitassignments { get; set; } = new List<Taskunitassignment>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
