using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Paginations;
using Microsoft.AspNetCore.Mvc;
using TaskModel = DocTask.Core.Models.Task;


namespace DocTask.Api.Controllers;

[ApiController]
[Route("/api/v1/task")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PageOptionsRequest pageOptions)
    {
        var tasks = await _taskService.GetAll(pageOptions);
        return Ok(new ApiResponse<PaginatedList<TaskDto>>
        {
            Data = tasks,
            Message = "Get All Tasks Successfully",
        });
    }
    // GET: api/tasks/{taskId}
    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetSubtasks(int taskId, [FromQuery] PageOptionsRequest pageOptions, string? search = null)
    {
        try
        {
            var subtasks = await _taskService.GetSubtasksAsync(taskId, pageOptions, search);

            var response = new ApiResponse<PaginatedList<TaskDto>>
            {
                Success = true,
                Data = subtasks,
                Message = "Lấy danh sách task con thành công."
            };

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Error = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<string>
            {
                Success = false,
                Error = ex.Message
            });
        }
    }
    // POST: api/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
    {
        try
        {
            var createdTask = await _taskService.CreateTaskAsync(taskDto);
            var response = new ApiResponse<TaskModel>
            {
                Success = true,
                Data = createdTask,
                Message = "Tạo task thành công."
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<string>
            {
                Success = false,
                Error = ex.Message
            });
        }
    }
    // PUT: api/tasks/{taskId}: xem chi tiết công việc cha - hiển thị danh sách công việc con
    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] TaskDto taskDto)
    {
        try
        {
            var updatedTask = await _taskService.UpdateTaskAsync(taskId, taskDto);
            if (updatedTask == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Error = "Task not found"
                });
            }

            var response = new ApiResponse<TaskModel>
            {
                Success = true,
                Data = updatedTask,
                Message = "Cập nhật task thành công."
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<string>
            {
                Success = false,
                Error = ex.Message
            });
        }
    }

    // DELETE: api/tasks/{taskId}
    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var (success, message) = await _taskService.DeleteTaskAsync(taskId);

        if (!success)
            return BadRequest(new { message });

        return NoContent();
    }

    // Assigner & Assignee
    [HttpGet("assigner/{assignerId}")]
    public async Task<IActionResult> GetTasksByAssignerId(int assignerId, [FromQuery] PageOptionsRequest pageOptions)
    {
        var tasks = await _taskService.GetTasksByAssignerId(assignerId, pageOptions);
        return Ok(new ApiResponse<PaginatedList<TaskDto>>
        {
            Data = tasks,
            Message = "Get tasks by assigner ID successfully",
        });
    }
    
    [HttpGet("assignee/{assigneeId}")]
    public async Task<IActionResult> GetTasksByAssigneeId(int assigneeId, [FromQuery] PageOptionsRequest pageOptions)
    {
        var tasks = await _taskService.GetTasksByAssigneeId(assigneeId, pageOptions);
        return Ok(new ApiResponse<PaginatedList<TaskDto>>
        {
            Data = tasks,
            Message = "Get tasks by assignee ID successfully",
        });
    }
}