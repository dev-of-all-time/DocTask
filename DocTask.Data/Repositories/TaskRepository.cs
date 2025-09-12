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
  public async Task<TaskModel?> CreateTaskAsync(TaskDto taskDto)
  {
    var newTask = new TaskModel
    {
      Title = taskDto.Title,
      Description = taskDto.Description,
      StartDate = taskDto.StartDate ?? DateTime.UtcNow,
      DueDate = taskDto.DueDate ?? DateTime.UtcNow.AddDays(1),
    };
    _context.Tasks.Add(newTask);
    await _context.SaveChangesAsync();

    return newTask;
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
      existingTask.StartDate = taskDto.StartDate;
      existingTask.DueDate = taskDto.DueDate;

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