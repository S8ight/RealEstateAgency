using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.BLL.DTO.Request;

public class VerifyEmailRequest
{
    [Required]
    public string Token { get; set; }
}