using DocTask.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Task = DocTask.Core.Models.Task;

namespace DocTask.Data.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Task>> GetAllAsync()
    {
        return await _context.Tasks.ToListAsync();
    }
}