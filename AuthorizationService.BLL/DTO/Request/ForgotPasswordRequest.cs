using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.BLL.DTO.Request;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}