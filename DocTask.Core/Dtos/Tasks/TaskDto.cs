namespace DocTask.Core.Dtos.Tasks;

public class TaskDto
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

    public DateOnly? StartDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UnitId { get; set; }

    public int? FrequencyId { get; set; }

    public int? Percentagecomplete { get; set; }

    public int? ParentTaskId { get; set; }
}