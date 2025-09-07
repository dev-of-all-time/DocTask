using Microsoft.EntityFrameworkCore;

namespace DocTask.Core.Paginations;

public class PaginatedList <T>
{
    public PaginatedMetaData MetaData { get; set; } = new PaginatedMetaData();
    public List<T> Items { get; set; } = [];
    
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        MetaData.PageIndex = pageIndex;
        MetaData.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        MetaData.TotalItems = count;
        MetaData.CurrentItems = items.Count;
        Items.AddRange(items);
    }

    public PaginatedList()
    {
    }
}

public static class PaginatedListHelper 
{
    private const int DefaultPageSize = 0;
    private const int DefaultCurrentPage = 1;

    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> query, 
        PageOptionsRequest options)
    {
        options.Page = options.Page > 0 ? options.Page : DefaultCurrentPage;
        options.Size = options.Size > 0 ? options.Size : DefaultPageSize;
        

        var count = await query.CountAsync();
        var items = await query.Skip((options.Page - 1) * options.Size)
            .Take(options.Size)
            .ToListAsync();
        return new PaginatedList<T>(items, count, options.Page, options.Size);
    }
}