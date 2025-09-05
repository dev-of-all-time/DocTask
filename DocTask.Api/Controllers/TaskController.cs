using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Interfaces.Services;
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
    public async Task<IActionResult> GetAll()
    {
        List<TaskModel> tasks = await _taskService.GetAllAsync();
        return Ok(new ApiResponse<List<TaskModel>>
        {
            Data = tasks,
            Message = "All tasks were retrieved successfully",
        });
    }
}