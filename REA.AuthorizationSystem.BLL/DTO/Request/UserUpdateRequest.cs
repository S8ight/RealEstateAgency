using Microsoft.AspNetCore.Http;

namespace REA.AuthorizationSystem.BLL.DTO.Request;

public class UserUpdateRequest
{
    public string UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public IFormFile? Photo { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
}