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
    void DeleteTask(TaskModel task);
    Task SaveChangesAsync();
}