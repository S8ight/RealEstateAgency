using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.Interfaces;

namespace REA.ChatSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
        
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpGet("GetUser/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
    {
        try
        {
            await _userService.AddAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }
}