using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using REA.AuthorizationSystem.BLL.Authorization.Helpers;
using REA.AuthorizationSystem.BLL.Interfaces;

namespace REA.AuthorizationSystem.BLL.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtConfiguration jwtConfiguration)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtConfiguration.ValidateJwtToken(token);
        if (userId != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = userService.GetById(userId.Value.ToString());
        }

        await _next(context);
    }
}