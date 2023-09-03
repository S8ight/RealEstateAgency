using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.AdvertSystem.Application.Common.DTO.AdvertDTO;
using REA.AdvertSystem.Application.Common.DTO.UserDTO;
using REA.AdvertSystem.Application.Users.Commands;
using REA.AdvertSystem.Application.Users.Queries;

namespace REA.AdvertSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private IMediator _mediator = null!;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet("GetUser")]
    public async Task<ActionResult<UserResponse>> GetUserById([FromQuery] GetUserById request)
    {
        try
        {
            var user = await Mediator.Send(request);
            
            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while receiving user: {Id}", request.Id);
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteUser([FromQuery] DeleteUserCommand command)
    {
        try
        {
            await Mediator.Send(command);
            
            _logger.LogInformation("User Deleted: {Id}", command.Id);
            return Ok(await Mediator.Send(command));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while deleting user: {Id}", command.Id);
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut]
    public async Task<ActionResult<string>> UpdateUser(UpdateUserCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            
            _logger.LogInformation("User updated: {Id}", command.Id);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while updating user: {Id}", command.Id);
            return BadRequest(e.Message);
        }
    }
}