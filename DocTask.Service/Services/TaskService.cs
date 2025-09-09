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
}