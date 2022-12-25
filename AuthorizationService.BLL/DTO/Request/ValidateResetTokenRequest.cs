using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.BLL.DTO.Request;

public class ValidateResetTokenRequest
{
    [Required]
    public string Token { get; set; }
}