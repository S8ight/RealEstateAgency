namespace REA.AdvertSystem.Application.Common.Models;

public class PaginationResponse<T>
{
    public int PagesCount { get; set; }
    public IEnumerable<T> Items { get; set; }
}