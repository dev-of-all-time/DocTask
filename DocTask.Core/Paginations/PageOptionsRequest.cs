namespace DocTask.Core.Paginations;

public class PageOptionsRequest
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}