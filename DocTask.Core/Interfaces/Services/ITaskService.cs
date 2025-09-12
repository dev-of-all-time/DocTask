using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Paginations;
using TaskModel = DocTask.Core.Models.Task;

namespace DocTask.Core.Interfaces.Services;

public interface ITaskService
{
    // tasks
    Task<PaginatedList<TaskDto>> GetAll(PageOptionsRequest pageOptions);
    Task<TaskModel?> CreateTaskAsync(TaskDto taskDto);
    Task<TaskModel?> UpdateTaskAsync(int taskId, TaskDto taskDto);
    Task DeleteTaskAsync(int taskId);

    //assigner & assignee
    Task<PaginatedList<TaskDto>> GetTasksByAssignerId(int assignerId, PageOptionsRequest pageOptions);
    Task<PaginatedList<TaskDto>> GetTasksByAssigneeId(int assigneeId, PageOptionsRequest pageOptions);
}