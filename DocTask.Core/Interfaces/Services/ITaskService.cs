using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Paginations;
using TaskModel = DocTask.Core.Models.Task;

namespace DocTask.Core.Interfaces.Services;

public interface ITaskService
{
    Task<PaginatedList<TaskDto>> GetAll(PageOptionsRequest pageOptions);
    Task<TaskModel?> GetTaskByIdAsync(int taskId);
    Task<PaginatedList<TaskDto>> GetSubtasksAsync(int parentTaskId, PageOptionsRequest pageOptions, string? search = null);
    Task<TaskModel> AddSubtaskAsync(int parentTaskId, TaskDto subtaskDto);
}