using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.Interfaces;

namespace REA.ChatSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
        
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var data = await _userService.GetAsync(id);
            return Ok(data);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] UserRequest request)
    {
        try
        {
            var data = await _userService.AddAsync(request);
            return Ok(data);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserRequest request)
    {
        try
        {
            await _userService.ReplaceAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
        
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}