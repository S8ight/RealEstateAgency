using AuthorizationService.BLL.Extensions.Helpers;
using AuthorizationService.BLL.Interfaces;
using AuthorizationService.DAL.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AuthorizationService.BLL.Extensions.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, AuthorizationContext AuthContext, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var accountId = jwtUtils.ValidateJwtToken(token);
        if (accountId != null)
        {
            // attach account to context on successful jwt validation
            context.Items["Account"] = await AuthContext.User.FindAsync(accountId);
        }

        await _next(context);
    }
}