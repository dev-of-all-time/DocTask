using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Paginations;
using Microsoft.EntityFrameworkCore;
using TaskModel = DocTask.Core.Models.Task;

namespace DocTask.Data.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<TaskModel>> GetAllAsync(PageOptionsRequest pageOptions)
    {
        var query = _context.Tasks.Where(t => t.ParentTaskId == null).AsQueryable();
        return await query.ToPaginatedListAsync(pageOptions);
    }
}