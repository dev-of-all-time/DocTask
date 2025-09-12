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

}
