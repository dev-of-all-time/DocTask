using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Paginations;
using Microsoft.AspNetCore.Mvc;
using TaskModel = DocTask.Core.Models.Task;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DocTask.Data;
using DocTask.Core.Exceptions;
namespace DocTask.Api.Controllers;

[ApiController]
[Route("/api/v1/task")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ApplicationDbContext _dbContext;

    public TaskController(ITaskService taskService, ApplicationDbContext dbContext)
    {
        _taskService = taskService;
        _dbContext = dbContext;
    }

    // GET: api/v1/task
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PageOptionsRequest pageOptions)
    {
        var tasks = await _taskService.GetAll(pageOptions);
        return Ok(new ApiResponse<PaginatedList<TaskDto>>
        {
            Data = tasks,
            Message = "Get all tasks successfully."
        });
    }

    // POST: api/v1/task
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
    {

        var createdTask = await _taskService.CreateTaskAsync(taskDto);
        return Ok(new ApiResponse<TaskModel>
        {
            Success = true,
            Data = createdTask,
            Message = "Tạo task thành công."
        });
    }

    // PUT: api/v1/task/{taskId}
    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] TaskDto taskDto)
    {
        var updatedTask = await _taskService.UpdateTaskAsync(taskId, taskDto);
        return Ok(new ApiResponse<TaskModel>
        {
            Success = true,
            Data = updatedTask,
            Message = "Cập nhật task thành công."
        });
    }

    // DELETE: api/v1/task/{taskId}
    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        await _taskService.DeleteTaskAsync(taskId);
        return Ok(new ApiResponse<string>
        {
            Success = true,
            Data = null,
            Message = "Xóa task thành công."
        });
    }

    // GET: api/v1/task/{assignmentId}/assignment
    [HttpGet("assigner/{assignerId}")]
    public async Task<IActionResult> GetTasksByAssignerId(int assignerId, [FromQuery] PageOptionsRequest pageOptions)
    {
        var tasks = await _taskService.GetTasksByAssignerId(assignerId, pageOptions);
        return Ok(new ApiResponse<PaginatedList<TaskDto>>
        {
            Data = tasks,
            Message = "Lấy danh sách task theo người giao thành công",
        });
    }
    [HttpGet("assigneeid")]
    public async Task<IActionResult> GetMyTasks()
    {
        var username = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized(new ApiResponse<string> { Success = false, Error = "Không thể xác thực người dùng." });
        }

        var userId = await _dbContext.Users
            .Where(u => u.Username == username)
            .Select(u => u.UserId)
            .FirstOrDefaultAsync();

        if (userId == 0)
        {
            throw new NotFoundException("Người dùng không tồn tại.");
        }

        var tasks = await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.Users.Any(u => u.UserId == userId))
            .Select(t => new TaskDto
            {
                
                Title = t.Title,
                Description = t.Description,
                
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                
            })
            .ToListAsync();

        return Ok(new ApiResponse<List<TaskDto>>
        {
            Data = tasks,
            Message = "Lấy danh sách công việc theo người dùng thành công."
        });
    }
}
