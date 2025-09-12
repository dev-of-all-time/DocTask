using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Exceptions;
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

        if (paginatedListModel == null || !paginatedListModel.Items.Any())
        {
            throw new NotFoundException("Không tìm thấy task nào.");
        }

        return new PaginatedList<TaskDto>
        {
            MetaData = paginatedListModel.MetaData,
            Items = paginatedListModel.Items.Select(t => t.ToTaskDto()).ToList(),
        };
    }

    public async Task<TaskModel> CreateTaskAsync(TaskDto taskDto)
    {
        if (string.IsNullOrWhiteSpace(taskDto.Title))
        {
            throw new BadRequestException("Task title không được để trống.");
        }
        if (string.IsNullOrWhiteSpace(taskDto.Description))
        {
            throw new BadRequestException("Mô tả không được để trống.");
        }

        var created = await _taskRepository.CreateTaskAsync(taskDto);

        if (created == null)
        {
            throw new InternalServerErrorException("Không thể tạo task.");
        }

        return created;
    }

    public async Task<TaskModel> UpdateTaskAsync(int taskId, TaskDto taskDto)
    {
        var existingTask = await _taskRepository.GetTaskByIdAsync(taskId);
        if (existingTask == null)
        {
            throw new NotFoundException($"Không tìm thấy task với ID {taskId}.");
        }

        var updated = await _taskRepository.UpdateTaskAsync(taskId, taskDto);
        if (updated == null)
        {
            throw new InternalServerErrorException("Cập nhật task thất bại.");
        }

        return updated;
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        if (task == null)
        {
            throw new NotFoundException($"Không tìm thấy task với ID {taskId}.");
        }

        // Nếu là task cha
        if (task.ParentTaskId == null)
        {
            var subtaskCount = await _taskRepository.CountSubtasksAsync(task.TaskId);
            if (subtaskCount > 0)
            {
                throw new ConflictException("Không thể xóa task cha vì còn task con. Xóa tất cả task con trước.");
            }
        }

        _taskRepository.DeleteTask(task);
        await _taskRepository.SaveChangesAsync();
    }
}
