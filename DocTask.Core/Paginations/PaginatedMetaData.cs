namespace DocTask.Core.Paginations;

public record PaginatedMetaData
{
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public int CurrentItems {get; set;}
    public bool HasPrevious => PageIndex > 1;
    public bool HasNext => PageIndex < TotalPages;
}