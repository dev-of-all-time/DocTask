using System.Security.Claims;
using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.DTOs.Reminders;
using DocTask.Core.Exceptions;
using DocTask.Core.Models;
using DocTask.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Api.Controllers;

[ApiController]
[Route("/api/v1/reminder")]
public class ReminderController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public ReminderController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("user-reminders")]
    public async Task<IActionResult> GetUserRemindersAsync()
    {
        var username = GetUsernameFromHttpContext();
        
        if (string.IsNullOrEmpty(username))
        {
            throw new UnauthorizedException("Không thể xác thực người dùng.");
        }

        var user = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundException("Người dùng không tồn tại.");
        }

        var reminders = await _dbContext.Reminders
            .AsNoTracking()
            .Where(r => r.UserId == user.UserId)
            .Include(r => r.Task)
            .Include(r => r.Period)
            .Include(r => r.Notification)
            .Select(r => new
            {
                r.Reminderid,
                r.Title,
                r.Message,
                r.Isnotified,
                Task = new
                {
                    r.Task.TaskId,
                    r.Task.Title,
                    r.Task.Description,
                    r.Task.Status,
                    r.Task.StartDate,
                    r.Task.DueDate
                },
                Period = r.Period != null ? new
                {
                    r.Period.PeriodId,
                    r.Period.PeriodName
                } : null,
                Notification = r.Notification != null ? new
                {
                    r.Notification.NotificationId,
                    r.Notification.Message,
                    r.Notification.IsRead,
                    r.Notification.CreatedAt
                } : null
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Data = reminders,
            Message = "Lấy danh sách nhắc nhở thành công."
        });
    }

    [HttpPost("create/{taskId}/{userId}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> CreateReminderAsync(int taskId, int userId, [FromBody] CreateReminderRequestDto request)
    {
        // Check if user exists
        var user = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserId == userId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundException("Người dùng không tồn tại.");
        }

        // Check if task exists and belongs to the user
        var task = await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.TaskId == taskId )
            .FirstOrDefaultAsync();

        if (task == null)
        {
            throw new NotFoundException("Nhiệm vụ không tồn tại hoặc không thuộc về người dùng này.");
        }

        var reminder = new Reminder
        {
            Taskid = taskId,
            Message = request.Message,
            UserId = userId,
            Triggertime = DateTime.Now,
            Createdby = userId,
            Createdat = DateTime.Now,
            Title = request.Message, // Using message as title
            Isauto = false,
            Isnotified = false
        };

        _dbContext.Reminders.Add(reminder);
        await _dbContext.SaveChangesAsync();

        return Ok(new ApiResponse<object>
        {
            Data = new
            {
                reminder.Reminderid,
                reminder.Title,
                reminder.Message,
                reminder.Triggertime,
                reminder.Createdat,
                TaskId = reminder.Taskid,
                UserId = reminder.UserId
            },
            Message = "Tạo nhắc nhở thành công."
        });
    }

    [HttpDelete("delete/{reminderId}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> DeleteReminderAsync(int reminderId)
    {
        var reminder = await _dbContext.Reminders
            .Where(r => r.Reminderid == reminderId)
            .FirstOrDefaultAsync();

        if (reminder == null)
        {
            throw new NotFoundException("Nhắc nhở không tồn tại.");
        }

        _dbContext.Reminders.Remove(reminder);
        await _dbContext.SaveChangesAsync();

        return Ok(new ApiResponse<object>
        {
            Data = new
            {
                reminderId = reminder.Reminderid,
                message = "Nhắc nhở đã được xóa thành công."
            },
            Message = "Xóa nhắc nhở thành công."
        });
    }    
    private string? GetUsernameFromHttpContext()
    {
        var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }
}
