namespace REA.AdvertSystem.DTOs.Request;

public class UserSaveListRequest : PaginationRequest
{
    public string UserId { get; set; }
}