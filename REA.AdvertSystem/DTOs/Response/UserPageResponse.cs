namespace REA.AdvertSystem.DTOs.Response;

public class UserPageResponse
{
    public string FirstName { get; set; }
        
    public string LastName { get; set; }
        
    public string? Patronymic { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public List<AdvertsListResponse> UserAdverts { get; set; }
}