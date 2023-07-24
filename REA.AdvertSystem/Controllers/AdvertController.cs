using MediatR;
using Microsoft.AspNetCore.Mvc;
using REA.AdvertSystem.Application.Adverts.Commands;
using REA.AdvertSystem.Application.Adverts.Queries;
using REA.AdvertSystem.Application.Common.DTO.AdvertDTO;
using REA.AdvertSystem.Application.Common.GrpcServices;
using REA.AdvertSystem.Application.Common.Models;

namespace REA.AdvertSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdvertController : ControllerBase
{
    private IMediator _mediator = null!;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    private DiscountServiceGrpc _discountServiceGrpc;
    
    public AdvertController(DiscountServiceGrpc discountServiceGrpc)
    {
        _discountServiceGrpc = discountServiceGrpc;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<string>> Create(CreateAdvertCommand command)
    {
        return await Mediator.Send(command);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<PaginatedList<AdvertResponse>>> GetAllAdverts(
        [FromQuery] GetAdvertsPaginationList query)
    {
        try
        {
            var adverts = await Mediator.Send(query);

            foreach (var advert in adverts.Items)
            {
                var newPrice = await _discountServiceGrpc.GetDiscount(advert.AdvertID, advert.Price);
                advert.Price = newPrice.CalculatedPrice;
            }
            return Ok(adverts);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<ActionResult<AdvertResponse>> GetById(string id)
    {
        try
        {
            GetAdvertsById query = new GetAdvertsById() { Id = id };
            var advert = await Mediator.Send(query);
            var newPrice = await _discountServiceGrpc.GetDiscount(id, advert.Price);
            advert.Price = newPrice.CalculatedPrice;
            return Ok(advert);
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
    public async Task<ActionResult<string>> Delete([FromQuery] DeleteAdvertCommand command)
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
    [HttpPut]
    public async Task<ActionResult<string>> Update(UpdateAdvertCommand command)
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
