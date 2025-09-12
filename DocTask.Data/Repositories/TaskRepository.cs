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

  public async Task<TaskModel?> CreateTaskAsync(TaskDto taskDto)
  {
    var newTask = new TaskModel
    {
      Title = taskDto.Title,
      Description = taskDto.Description,
      AssignerId = taskDto.AssignerId,
      AssigneeId = taskDto.AssigneeId,
      OrgId = taskDto.OrgId,
      PeriodId = taskDto.PeriodId,
      AttachedFile = taskDto.AttachedFile,
      Status = taskDto.Status ?? "New",
      Priority = taskDto.Priority ?? "Normal",
      StartDate = taskDto.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
      DueDate = taskDto.DueDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
      CreatedAt = DateTime.UtcNow,
      UnitId = taskDto.UnitId,
      FrequencyId = taskDto.FrequencyId,
      Percentagecomplete = taskDto.Percentagecomplete ?? 0,
      ParentTaskId = taskDto.ParentTaskId
    };
    return await Task.FromResult(newTask);
  }

  public async Task<TaskModel?> UpdateTaskAsync(int taskId, TaskDto taskDto)
  {
    var existingTask = await _context.Tasks
                    .FirstOrDefaultAsync(t => t.TaskId == taskId);
      if (existingTask == null)
        return null;

      // Cập nhật thông tin cơ bản
      existingTask.Title = taskDto.Title;
      existingTask.Description = taskDto.Description;
      existingTask.AssignerId = taskDto.AssignerId;
      existingTask.AssigneeId = taskDto.AssigneeId;
      existingTask.OrgId = taskDto.OrgId;
      existingTask.PeriodId = taskDto.PeriodId;
      existingTask.AttachedFile = taskDto.AttachedFile;
      existingTask.Status = taskDto.Status;
      existingTask.Priority = taskDto.Priority;
      existingTask.StartDate = taskDto.StartDate;
      existingTask.DueDate = taskDto.DueDate;
      existingTask.UnitId = taskDto.UnitId;
      existingTask.FrequencyId = taskDto.FrequencyId;
      existingTask.Percentagecomplete = taskDto.Percentagecomplete;

    existingTask.ParentTaskId = taskDto.ParentTaskId;


    _context.Tasks.Update(existingTask);
      await _context.SaveChangesAsync();

      return existingTask;
  }


  public Task<bool> DeleteTaskAsync(int taskId)
  {
    try
    {
      var task = _context.Tasks.Find(taskId);
      if (task == null)
        return Task.FromResult(false);

      _context.Tasks.Remove(task);
      _context.SaveChanges();
      return Task.FromResult(true);
    }
    catch (Exception)
    {
      return Task.FromResult(false);
    }
  }

  public async Task<int> CountSubtasksAsync(int TaskId, string? search = null)
  {
    var query = _context.Tasks.Where(t => t.ParentTaskId == TaskId);
    if (!string.IsNullOrEmpty(search))
      query = query.Where(t => t.Title.Contains(search));
    return await query.CountAsync();
  }
  
  public async Task<TaskModel?> GetSubtaskByIdAsync(int parentTaskId, int taskId)
  {
    return await _context.Tasks
                // .Include(t => t.Notifications)
                // .Include(t => t.Progresses)
                // .Include(t => t.Reminders)
                .FirstOrDefaultAsync(t => t.TaskId == taskId && t.ParentTaskId == parentTaskId);
  }

  public void DeleteTask(TaskModel task)
  {
    _context.Tasks.Remove(task);
  }

  public async Task SaveChangesAsync()
  {
    await _context.SaveChangesAsync();
  }
}