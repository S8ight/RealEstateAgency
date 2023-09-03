using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using REA.AuthorizationSystem.BLL.Interfaces;
using REA.AuthorizationSystem.DAL.Entities;

namespace REA.AuthorizationSystem.BLL.Services;

public class TokenService: ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _accessTokenKey;
    private readonly SymmetricSecurityKey _refreshTokenKey;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _accessTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:AccessTokenKey"] ?? string.Empty));
        _refreshTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:RefreshTokenKey"] ?? string.Empty));
    }

    public string CreateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(30).ToString(CultureInfo.InvariantCulture)),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture))
        };

        var credentials = new SigningCredentials(_accessTokenKey, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(30),
            SigningCredentials = credentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var accessToken = tokenHandler.WriteToken(token);

        _logger.LogInformation("Access token created for user: {UserName}", user.UserName);

        return accessToken;
    }


    public string CreateRefreshToken(User user)
    {
        var refreshTokenClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpirationMinutes"])).ToString(CultureInfo.InvariantCulture))
        };

        var refreshTokenCredentials = new SigningCredentials(_refreshTokenKey, SecurityAlgorithms.HmacSha512Signature);

        var refreshTokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(refreshTokenClaims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpirationMinutes"])),
            SigningCredentials = refreshTokenCredentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var refreshTokenHandler = new JwtSecurityTokenHandler();
        var token = refreshTokenHandler.CreateToken(refreshTokenDescriptor);
        
        var refreshToken = refreshTokenHandler.WriteToken(token);

        _logger.LogInformation("Refresh token created for user: {UserName}", user.UserName);
        return refreshToken;
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token, bool isAccessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null)
        {
            return null!;
        }

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            IssuerSigningKey = isAccessToken? _accessTokenKey : _refreshTokenKey
        };

        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

        return principal;
    }

}