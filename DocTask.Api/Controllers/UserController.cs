using System.Security.Claims;
using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Exceptions;
using DocTask.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Api.Controllers;

[ApiController]
[Route("/api/v1/user")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public UserController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("getsubordinate")]
    public async Task<IActionResult> GetSubordinateAsync([FromQuery] int? id = null)
    {
        var callerUsername = id != null ? 
            await _dbContext.Users.Where(u => u.UserId == id).Select(u => u.Username).FirstOrDefaultAsync() :
            GetUsernameFromHttpContext();
            
        if (callerUsername == null)
        {
            throw new NotFoundException("Người dùng không tồn tại.");
        }

        var callerParent = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Username == callerUsername)
            .Select(u => u.UserParent)
            .FirstOrDefaultAsync();

        var subordinates = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserParentNavigation != null && u.UserParentNavigation.Username == callerUsername)
            .Select(u => new
            {
                u.UserId,
                u.Username,
                u.FullName,
                u.Email,
                u.OrgId,
                u.UnitId,
                u.Role,
            })
            .ToListAsync();

        var peers = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserParent == callerParent && u.Username != callerUsername)
            .Select(u => new
            {
                u.UserId,
                u.Username,
                u.FullName,
                u.Email,
                u.OrgId,
                u.UnitId,
                u.Role,
            })
            .ToListAsync();

        var result = new
        {
            Subordinates = subordinates,
            Peers = peers
        };

        return Ok(new ApiResponse<object>
        {
            Data = result,
            Message = "Lấy danh sách cấp dưới và đồng nghiệp thành công."
        });
    }

    private string? GetUsernameFromHttpContext()
    {
        var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }
}


