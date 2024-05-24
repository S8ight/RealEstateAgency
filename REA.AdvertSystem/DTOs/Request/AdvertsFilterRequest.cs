using REA.AdvertSystem.DataAccess.Entities.Enums;

namespace REA.AdvertSystem.DTOs.Request;

public class AdvertsFilterRequest : PaginationRequest
{
    public string? Province { get; set; }
    public string? Settlement { get; set; }
    public float? Square { get; set; }
    public int? FloorsNumber { get; set; }
    public int? RoomsNumber { get; set; }
    public float? MinPrice { get; set; }
    public float? MaxPrice { get; set; }
    public EstateType? EstateType { get; set; }
}