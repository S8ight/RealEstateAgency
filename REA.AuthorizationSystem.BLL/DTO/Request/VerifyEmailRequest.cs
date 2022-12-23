using System.ComponentModel.DataAnnotations;

namespace REA.AuthorizationSystem.BLL.DTO.Request;

public class VerifyEmailRequest
{
    [Required]
    public string Token { get; set; }
    public string Email { get; set; }
}