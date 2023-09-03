using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using REA.AdvertSystem.Application.Common.DTO.SaveListDTO;
using REA.AdvertSystem.Application.Common.Extensions;
using REA.AdvertSystem.Application.SaveLists.Commands;
using REA.AdvertSystem.Application.SaveLists.Queries;

namespace REA.AdvertSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SaveListController : ControllerBase
{
    private IMediator _mediator = null!;
    private readonly IDistributedCache _cache;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    
    private readonly ILogger<SaveListController> _logger;

    public SaveListController(IDistributedCache cache, ILogger<SaveListController> logger)
    {
        _cache = cache;
        _logger = logger;
    }
    
    [HttpPost("CreateSaveList")]
    public async Task<ActionResult<string>> CreateSaveList(CreateSaveListRecordCommand recordCommand)
    {
        try
        {
            return Ok(await Mediator.Send(recordCommand));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while creating SaveList");
            return BadRequest(e.Message);
        }
    }

    
    [HttpGet("GetSaveListAdverts")]
    public async Task<ActionResult<List<SaveListResponse>>> GetSaveListAdverts([FromQuery] GetSaveListbyUser query)
    {
        string recordKey = $"SaveList_User_{query.Id}";
        try
        {
            List<SaveListResponse>? response = await _cache.GetRecordAsync<List<SaveListResponse>>(recordKey);

            if (response is null)
            {
                response = await Mediator.Send(query);
                await _cache.SetRecordAsync(recordKey, response);
            }

            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error occurred while receiving adverts from SaveList");
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("DeleteSaveListRecord")]
    public async Task<ActionResult<string>> DeleteSaveListRecord(DeleteSaveListRecordCommand recordCommand)
    {
        try
        {
            var id = await Mediator.Send(recordCommand);
            
            await _cache.RemoveAsync($"SaveList_User_{id}");
            
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while deleting SaveList");
            return BadRequest(e.Message);
        }
    }
}