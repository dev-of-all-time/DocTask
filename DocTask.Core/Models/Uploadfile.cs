namespace DocTask.Core.Models;

public partial class Uploadfile
{
    public int FileId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public int? UploadedBy { get; set; }

    public DateTime UploadedAt { get; set; }

    public virtual ICollection<Reportsummary> Reportsummaries { get; set; } = new List<Reportsummary>();

    public virtual User? UploadedByNavigation { get; set; }
}
