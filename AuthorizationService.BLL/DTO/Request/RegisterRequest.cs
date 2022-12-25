using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.BLL.DTO.Request;

public class RegisterRequest
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    
    [Required]
    public DateTime DateOfBirthd { get; set; }
    public byte[]? Photo { get; set; }
}