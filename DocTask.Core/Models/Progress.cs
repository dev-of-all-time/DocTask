namespace DocTask.Core.Models;

public partial class Progress
{
    public int ProgressId { get; set; }

    public int TaskId { get; set; }

    public int? PeriodId { get; set; }

    public int? PercentageComplete { get; set; }

    public string? Comment { get; set; }

    public string? Status { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public string? Proposal { get; set; }

    public string? Result { get; set; }

    public string? Feedback { get; set; }

    public virtual Period? Period { get; set; }

    public virtual ICollection<ReportReview> ReportReviews { get; set; } = new List<ReportReview>();

    public virtual Task Task { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
