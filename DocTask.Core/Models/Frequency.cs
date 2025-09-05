namespace DocTask.Core.Models;

public partial class Frequency
{
    public int FrequencyId { get; set; }

    public string FrequencyType { get; set; } = null!;

    public string? FrequencyDetail { get; set; }

    public int IntervalValue { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<FrequencyDetail> FrequencyDetails { get; set; } = new List<FrequencyDetail>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
