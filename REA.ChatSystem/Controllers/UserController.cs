using Microsoft.AspNetCore.Mvc;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.Interfaces;

namespace REA.ChatSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
        
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var data = await _userService.GetAsync(id);
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] UserRequest request)
    {
        var data = await _userService.AddAsync(request);
        return Ok(data);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserRequest request)
    {
        await _userService.ReplaceAsync(request);
        return Ok();
    }
        
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        await _userService.DeleteAsync(id);
        return Ok();
    }
}