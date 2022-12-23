using REA.AuthorizationSystem.DAL.Entities;

namespace REA.AuthorizationSystem.BLL.Interfaces;

public interface IJwtConfiguration
{
    public string GenerateJwtToken(User user);
    public int? ValidateJwtToken(string token);
    public RefreshToken GenerateRefreshToken(string ipAddress);
}