using System.Security.Claims;
using REA.AuthorizationSystem.DAL.Entities;

namespace REA.AuthorizationSystem.BLL.Interfaces;

public interface ITokenService
{
    public string CreateAccessToken(User user);
    public string CreateRefreshToken(User user);
    public ClaimsPrincipal GetPrincipalFromToken(string token, bool isAccessToken);
}