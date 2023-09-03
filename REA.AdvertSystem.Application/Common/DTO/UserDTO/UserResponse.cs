using Microsoft.AspNetCore.Mvc;

namespace REA.AdvertSystem.Application.Common.DTO.UserDTO;

public class UserResponse
{
    public string Id { get; set; }

    public string FirstName { get; set; }
        
    public string LastName { get; set; }
        
    public string? Patronymic { get; set; }
        
    public FileContentResult? Photo { get; set; }
}