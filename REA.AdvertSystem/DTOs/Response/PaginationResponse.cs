namespace REA.AdvertSystem.DTOs.Response;

public class PaginationResponse<T>
{
    public int PagesCount { get; set; }
    public IEnumerable<T> Items { get; set; }
}