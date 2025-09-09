using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Paginations;
using Microsoft.EntityFrameworkCore;
using TaskModel = DocTask.Core.Models.Task;

namespace DocTask.Data.Repositories;

public class TaskRepository : ITaskRepository
{
  private readonly ApplicationDbContext _context;

  public TaskRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<PaginatedList<TaskModel>> GetAllAsync(PageOptionsRequest pageOptions)
  {
    var query = _context.Tasks.Where(t => t.ParentTaskId == null).AsQueryable();
    return await query.ToPaginatedListAsync(pageOptions);
  }

  public async Task<TaskModel?> GetTaskByIdAsync(int taskId)
  {
    return await _context.Tasks
        .Include(t => t.Assignee)
        .FirstOrDefaultAsync(t => t.TaskId == taskId);
  }

  public async Task<TaskModel> AddSubtaskAsync(int parentTaskId, TaskDto subtaskDto)
  {
    var parentTask = await GetTaskByIdAsync(parentTaskId);
    if (parentTask == null)
      throw new KeyNotFoundException($"Task cha với Id {parentTaskId} không tồn tại.");

    var subtask = new TaskModel
    {
      Title = subtaskDto.Title,
      Description = subtaskDto.Description,
      AssignerId = parentTask.AssignerId,
      AssigneeId = subtaskDto.AssigneeId,
      ParentTaskId = parentTask.TaskId,
      OrgId = parentTask.OrgId,
      UnitId = subtaskDto.UnitId,
      FrequencyId = subtaskDto.FrequencyId,
      Status = subtaskDto.Status ?? "New",
      Priority = subtaskDto.Priority ?? "Normal",
      StartDate = subtaskDto.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
      DueDate = subtaskDto.DueDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
      CreatedAt = DateTime.UtcNow,
      Percentagecomplete = subtaskDto.Percentagecomplete ?? 0
    };

    _context.Tasks.Add(subtask);
    await _context.SaveChangesAsync();

    return subtask;
  }

  public async Task<PaginatedList<Core.Dtos.Tasks.TaskDto>> GetSubtasksAsync(int parentTaskId, PageOptionsRequest pageOptions, string? search = null)
  {
    var query = _context.Tasks
        .Where(t => t.ParentTaskId == parentTaskId)
        .Include(t => t.Assignee)
        .Select(t => new TaskDto
        {
          TaskId = t.TaskId,
          Title = t.Title,
          Description = t.Description,
          AssigneeId = t.AssigneeId,
          StartDate = t.StartDate,
          DueDate = t.DueDate,
          Status = t.Status,
          Priority = t.Priority,
          Percentagecomplete = t.Percentagecomplete
        });

    if (!string.IsNullOrEmpty(search))
      query = query.Where(t => t.Title.Contains(search));

    return await query.ToPaginatedListAsync(pageOptions);
  }

}