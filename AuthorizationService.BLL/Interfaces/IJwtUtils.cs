using AuthorizationService.DAL.Entities;

namespace AuthorizationService.BLL.Interfaces;

public interface IJwtUtils
{
    public string GenerateJwtToken(User user);
    public string? ValidateJwtToken(string token);
    public RefreshToken GenerateRefreshToken(string ipAddress);
}