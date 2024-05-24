using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.Interfaces.Services;

namespace REA.AdvertSystem.Controllers;

[ApiController]
[Route("api/v1")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : ControllerBase
{ 
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [AllowAnonymous]
    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPost("users")]
    public async Task<IActionResult> CreateUser(UserRequest request)
    {
        await _userService.AddUserAsync(request);
        return Ok();
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        await _userService.DeleteUser(id);
        return Ok();
    }
}