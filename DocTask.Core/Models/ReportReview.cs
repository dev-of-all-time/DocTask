namespace DocTask.Core.Models;

public partial class ReportReview
{
    public int ReviewId { get; set; }

    public int ProgressId { get; set; }

    public int? ReviewerId { get; set; }

    public string Status { get; set; } = null!;

    public string? Comment { get; set; }

    public DateTime ReviewedAt { get; set; }

    public virtual Progress Progress { get; set; } = null!;

    public virtual User? Reviewer { get; set; }
}
