using DocTask.Core.Paginations;
using TaskModel = DocTask.Core.Models.Task;

namespace DocTask.Core.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<PaginatedList<TaskModel>> GetAllAsync(PageOptionsRequest pageOptions);
}