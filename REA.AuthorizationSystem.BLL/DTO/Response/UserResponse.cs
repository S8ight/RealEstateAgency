using Microsoft.AspNetCore.Mvc;

namespace REA.AuthorizationSystem.BLL.DTO.Response;

public class UserResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}