using System.ComponentModel.DataAnnotations;

namespace DocTask.Core.Dtos.Tasks;

public class TaskDto
{
  [Required(ErrorMessage = "Tiêu đề là bắt buộc.")]
  public string Title { get; set; } = null!;

  [Required(ErrorMessage = "Mô tả là bắt buộc.")]
  public string Description { get; set; }

  public DateTime? StartDate { get; set; }

  public DateTime? DueDate { get; set; }
}