using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DocTask.Api.Controllers;

[ApiController]
[Route("/test")]

public class TestController : ControllerBase
{
    [HttpGet("/admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult Test()
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // return "Admin " + userName;
        return Ok("Admin" + userName);
    }
    
    [HttpGet("/manager")]
    [Authorize(Roles = "Manager")]
    public IActionResult Manager()
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // return "Admin " + userName;
        return Ok("Manager" + userName);
    }
    
    [HttpGet("/staff")]
    [Authorize(Roles = "Staff")]
    public IActionResult Staff()
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // return "Admin " + userName;
        return Ok("Staff" + userName);
    }
}