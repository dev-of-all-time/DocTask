using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Paginations;
using TaskModel = DocTask.Core.Models.Task;

namespace DocTask.Core.Interfaces.Repositories;

public interface ITaskRepository
{
    // tasks
    Task<PaginatedList<TaskModel>> GetAllAsync(PageOptionsRequest pageOptions);
    Task<TaskModel?> GetTaskByIdAsync(int taskId);
    Task<TaskModel?> CreateTaskAsync(TaskDto taskDto);
    Task<TaskModel?> UpdateTaskAsync(int taskId, TaskDto taskDto);
    Task<bool> DeleteTaskAsync(int taskId);
    Task<int> CountSubtasksAsync(int parentTaskId, string? search = null);

    // subtasks
    Task<PaginatedList<TaskDto>> GetSubtasksAsync(int parentTaskId, PageOptionsRequest pageOptions, string? search = null);
    Task<TaskModel> AddSubtaskAsync(int parentTaskId, TaskDto subtaskDto);
    Task<TaskModel?> GetSubtaskByIdAsync(int parentTaskId, int taskId);
    void DeleteTask(TaskModel task);
    Task SaveChangesAsync();

    //assigner & assignee
    Task<PaginatedList<TaskModel>> GetTasksByAssignerId(int assignerId, PageOptionsRequest pageOptions);
    Task<PaginatedList<TaskModel>> GetTasksByAssigneeId(int assigneeId, PageOptionsRequest pageOptions);
}