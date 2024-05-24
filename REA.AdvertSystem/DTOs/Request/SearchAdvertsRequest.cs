namespace REA.AdvertSystem.DTOs.Request;

public class SearchAdvertsRequest : PaginationRequest
{
    public string Keywords { get; set; }
}
