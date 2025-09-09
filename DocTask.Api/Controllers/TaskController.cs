using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.Tasks;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Paginations;
using Microsoft.AspNetCore.Mvc;
using TaskModel = DocTask.Core.Models.Task;

namespace DockTask.Api.Controllers;

[ApiController]
[Route("/api/v1/task")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

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
    // GET: api/tasks/{taskId}/subtasks
    [HttpGet("{taskId}/subtasks")]
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

    // POST: api/tasks/{taskId}/subtasks
    [HttpPost("{taskId}/subtasks")]
    public async Task<IActionResult> AddSubtask(int taskId, [FromBody] TaskDto subtaskDto)
    {
        try
        {
            var subtask = await _taskService.AddSubtaskAsync(taskId, subtaskDto);

            var response = new ApiResponse<TaskModel>
            {
                Success = true,
                Data = subtask,
                Message = "Thêm task con thành công."
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
}