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
}