using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace REA.AuthorizationSystem.DAL.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public DateTime DateOfBirthd { get; set; }
    public DateTime Created { get; set; }
    public byte[]? Photo { get; set; }
        
    [JsonIgnore]
    public List<RefreshToken> RefreshTokens { get; set; }
}