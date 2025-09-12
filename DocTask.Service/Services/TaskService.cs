using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Paginations;
using DocTask.Service.Mappers;
using TaskModel = DocTask.Core.Models.Task;


namespace DocTask.Service.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<PaginatedList<TaskDto>> GetAll(PageOptionsRequest pageOptions)
    {
        var paginatedListModel = await _taskRepository.GetAllAsync(pageOptions);
        return new PaginatedList<TaskDto>
        {
            MetaData = paginatedListModel.MetaData,
            Items = paginatedListModel.Items.Select(t => t.ToTaskDto()).ToList(),
        };
    }
    
    public Task<TaskModel?> GetTaskByIdAsync(int taskId) => _taskRepository.GetTaskByIdAsync(taskId);
    public Task<PaginatedList<TaskDto>> GetSubtasksAsync(int parentTaskId, PageOptionsRequest pageOptions, string? search = null)
        => _taskRepository.GetSubtasksAsync(parentTaskId, pageOptions, search);

    public Task<TaskModel> AddSubtaskAsync(int parentTaskId, TaskDto subtaskDto)
        => _taskRepository.AddSubtaskAsync(parentTaskId, subtaskDto);

    public Task<TaskModel> CreateTaskAsync(TaskDto taskdto)
    {
        return _taskRepository.CreateTaskAsync(taskdto);
    }

    public Task<TaskModel?> UpdateTaskAsync(int taskId, TaskDto taskDto)
    {
        return _taskRepository.UpdateTaskAsync(taskId, taskDto);
    }

    public async Task<(bool Success, string? Message)> DeleteTaskAsync(int taskId)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        if (task == null)
            return (false, "Không tìm thấy task.");

        // Nếu là task cha
        if (task.ParentTaskId == null)
        {
            var subtaskCount = await _taskRepository.CountSubtasksAsync(task.TaskId);
            if (subtaskCount > 0)
                return (false, "Không thể xóa task cha vì còn task con. Xóa tất cả task con trước.");
        }

        // Xóa task (task con hoặc task cha không còn con)
        _taskRepository.DeleteTask(task);
        await _taskRepository.SaveChangesAsync();
        return (true, null);
    }
}