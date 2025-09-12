using DocTask.Core.Dtos.Tasks;
using Task = DocTask.Core.Models.Task;

namespace DocTask.Service.Mappers;

public static class TaskMapper
{
    public static TaskDto ToTaskDto(this Task task)
    {
        return new TaskDto
        {
            Title = task.Title,
            Description = task.Description,
            StartDate = task.StartDate,
            DueDate = task.DueDate,
        };
    }
}