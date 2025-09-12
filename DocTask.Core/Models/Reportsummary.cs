namespace DocTask.Core.Models;

public partial class Reportsummary
{
    public int ReportId { get; set; }

    public int? TaskId { get; set; }

    public int? PeriodId { get; set; }

    public string? Summary { get; set; }

    public int? CreatedBy { get; set; }

    public int? ReportFile { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Period? Period { get; set; }

    public virtual Uploadfile? ReportFileNavigation { get; set; }

    public virtual Task? Task { get; set; }
}
