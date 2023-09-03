namespace REA.AuthorizationSystem.BLL.DTO.Request;

public class ResetPasswordRequest
{
    public string Email { get; set; }
    public string NewPassword { get; set; }
    public string Token { get; set; }
}