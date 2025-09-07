using DocTask.Core.Dtos.Tasks;
using Task = DocTask.Core.Models.Task;

namespace DocTask.Service.Mappers;

public static class TaskMapper
{
    public static TaskDto ToTaskDto(this Task task)
    {
        return new TaskDto
        {
            TaskId = task.TaskId,
            Title = task.Title,
            Description = task.Description,
            AssignerId = task.AssignerId,
            AssigneeId = task.AssigneeId,
            OrgId = task.OrgId,
            PeriodId = task.PeriodId,
            AttachedFile = task.AttachedFile,
            Status = task.Status,
            Priority = task.Priority,
            StartDate = task.StartDate,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UnitId = task.UnitId,
            FrequencyId = task.FrequencyId,
            Percentagecomplete = task.Percentagecomplete,
            ParentTaskId = task.ParentTaskId,
        };
    }
}