namespace DocTask.Core.Models;

public partial class FrequencyDetail
{
    public int Id { get; set; }

    public int FrequencyId { get; set; }

    public int? DayOfWeek { get; set; }

    public int? DayOfMonth { get; set; }

    public virtual Frequency Frequency { get; set; } = null!;
}
