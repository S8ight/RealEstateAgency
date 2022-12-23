using Newtonsoft.Json;
using REA.AuthorizationSystem.DAL.Entities;

namespace REA.AuthorizationSystem.BLL.DTO.Response;

public class AuthenticationResponse
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string? Patronymic { get; set; }
    public string Username { get; set; }
    public byte[] Photo { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore]
    public string RefreshToken { get; set; }
    
    public AuthenticationResponse(User user, string jwtToken, string refreshToken)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Patronymic = user.Patronymic;
        Username = user.UserName;
        Photo = user.Photo;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }
}