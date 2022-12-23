using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using REA.AdvertSystem.Application.Common.DTO.SaveListDTO;
using REA.AdvertSystem.Application.Common.Extensions;
using REA.AdvertSystem.Application.SaveLists.Commands;
using REA.AdvertSystem.Application.SaveLists.Queries;

namespace REA.AdvertSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SaveListController : ControllerBase
{
    private IMediator _mediator = null!;
    private IDistributedCache cache;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    public SaveListController(IDistributedCache _cache)
    {
        cache = _cache;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<string>> Create(CreateSaveListCommand command)
    {
        try
        {
            return Ok(await Mediator.Send(command));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<List<SaveListResponse>>> GetAllAdverts([FromQuery] GetSaveListbyUser query)
    {
        List<SaveListResponse>? response;
        string recordKey = $"SaveList_User_{query.Id}";
        try
        {
            response = await cache.GetRecordAsync<List<SaveListResponse>>(recordKey);

            if (response is null)
            {
                response = await Mediator.Send(query);
                await cache.SetRecordAsync(recordKey, response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<ActionResult<string>> Delete(DeleteSaveListCommand command)
    {
        try
        {
            var id = await Mediator.Send(command);

            await cache.RemoveAsync($"SaveList_User_{id}");

            return Ok(id);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<ActionResult<string>> Update(UpdateSaveListCommand command)
    {
        try
        {
            return Ok(await Mediator.Send(command));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}