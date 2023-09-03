namespace REA.AuthorizationSystem.BLL.DTO.Response;

public class UserResponseByToken
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public byte[]? Photo { get; set; }
}