using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.AdvertSystem.Application.Common.DTO.PhotoListDTO;
using REA.AdvertSystem.Application.PhotoLists.Commands;
using REA.AdvertSystem.Application.PhotoLists.Queries;

namespace REA.AdvertSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PhotoListController : ControllerBase
{
    private IMediator _mediator = null!;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    
    private readonly ILogger<PhotoListController> _logger;

    public PhotoListController(ILogger<PhotoListController> logger)
    {
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpPost("CreatePhotoList")]
    public async Task<ActionResult<string>> CreatePhotoList(CreatePhotoListCommand command)
    {
        try
        {
            return Ok(await Mediator.Send(command));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while creating photo list");
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("GetAllAdvertPhotos")]
    public async Task<ActionResult<List<PhotoResponse>>> GetAllAdvertPhotos([FromQuery] GetAdvertPhotoList query)
    {
        try
        {
            return Ok(await Mediator.Send(query));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while receiving Advert({Id}) photos", query.Id);
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("DeletePhotoList")]
    public async Task<ActionResult<string>> DeletePhotoList(DeletePhotoListCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            
            _logger.LogInformation("Deleted photo: {Id}", command.Id);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while deleting photo");
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("UpdatePhotoList")]
    public async Task<ActionResult<string>> UpdatePhotoList(UpdatePhotoListCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            
            _logger.LogInformation("Updated PhotoList: {Id}", command.Id);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while updating photo");
            return BadRequest(e.Message);
        }
    }
}