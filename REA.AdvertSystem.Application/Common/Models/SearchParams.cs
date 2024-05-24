namespace REA.AdvertSystem.Application.Common.Models;

public class SearchParams : PaginationParams
{
    public string? SearchTerm { get; set; }
    public string? ColumnFilters { get; set; }
    public ColumnSorting? ColumnSorting { get; set; }
}