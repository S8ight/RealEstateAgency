using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.BLL.DTO.Request;

public class AuthenticateRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}