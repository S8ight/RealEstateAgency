using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using REA.AuthorizationSystem.BLL.DTO.Request;
using REA.AuthorizationSystem.BLL.Interfaces;

namespace REA.AuthorizationSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IBusControl _bus;

    public UserController(IUserService userService, IBusControl bus)
    {
        _userService = userService;
        _bus = bus;
    }
    
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthenticationRequest model)
    {
        var response = _userService.Authenticate(model, ipAddress());
        setTokenCookie(response.RefreshToken);
        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register(RegistrationRequest model)
    {
        var response = _userService.Register(model, Request.Headers["origin"]);
        if (response.Result.IsNullOrEmpty())
        {
            return Ok($"Registration successful, please check your email for verification instructions\n{response.Result}");
        }

        return BadRequest(response.Result);
    }
    
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public IActionResult RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = _userService.RefreshToken(refreshToken, ipAddress());
        setTokenCookie(response.RefreshToken);
        return Ok(response);
    }
    
    [HttpPost("revoke-token")]
    public IActionResult RevokeToken(RevokeTokenRequest model)
    {
        // accept refresh token in request body or cookie
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Token is required" });

        _userService.RevokeToken(token, ipAddress());
        return Ok(new { message = "Token revoked" });
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var user = _userService.GetById(id);
        return Ok(user);
    }

    [HttpGet("{id}/refresh-tokens")]
    public IActionResult GetRefreshTokens(string id)
    {
        var user = _userService.GetById(id);
        return Ok(user.Result.RefreshTokens);
    }
    
    [AllowAnonymous]
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail(VerifyEmailRequest model)
    {
        var verification = await _userService.VerifyEmail(model.Token, model.Email);
        await _userService.AddUserToQueue(model.Email);
        return Ok(verification);
    }
    
    private void setTokenCookie(string token)
    {
        // append cookie with refresh token to the http response
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string ipAddress()
    {
        // get source ip address for the current request
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }
}

