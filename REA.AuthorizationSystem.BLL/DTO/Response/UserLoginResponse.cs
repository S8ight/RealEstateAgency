namespace REA.AuthorizationSystem.BLL.DTO.Response;

public class UserLoginResponse
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public byte[]? Photo { get; set; }
}