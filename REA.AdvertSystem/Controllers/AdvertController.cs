using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.AdvertSystem.Application.Adverts.Commands;
using REA.AdvertSystem.Application.Adverts.Queries;
using REA.AdvertSystem.Application.Common.DTO.AdvertDTO;
using REA.AdvertSystem.Application.Common.GrpcServices;
using REA.AdvertSystem.Application.Common.Models;
using REA.AdvertSystem.Application.PhotoLists.Commands;
using REA.AdvertSystem.Application.PhotoLists.Queries;
using REA.AdvertSystem.Application.Users.Queries;

namespace REA.AdvertSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AdvertController : ControllerBase
{
    private IMediator _mediator = null!;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    private readonly DiscountClientGrpc _discountClientGrpc;
    
    private readonly ILogger<AdvertController> _logger;
    
    public AdvertController(DiscountClientGrpc discountClientGrpc, ILogger<AdvertController> logger)
    {
        _discountClientGrpc = discountClientGrpc;
        _logger = logger;
    }
    

    [AllowAnonymous]
    [HttpPost("CreateAdvert")]
    public async Task<ActionResult<string>> CreateAdvert(CreateAdvertCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);

            _logger.LogInformation("Created advert: {Id}", result);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while creating advert");
            return BadRequest(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpGet("GetAdvertsList")]
    public async Task<ActionResult<PaginatedList<AdvertListResponse>>> GetAdvertsList([FromQuery] GetAdvertsPaginationList query)
    {
        try
        {
            var adverts = await Mediator.Send(query);

            foreach (var advert in adverts.Items)
            {
                var discount = await _discountClientGrpc.GetDiscount(advert.Id, advert.Price);
                if (discount.ExpiresAt != null)
                {
                    advert.PriceWithDiscount = discount.CalculatedPrice;
                    advert.DiscountExpirationTime = discount.ExpiresAt.ToDateTime();
                }

                advert.PhotoUrl = await Mediator.Send(new GetAdvertPreviewPhotoList { Id = advert.Id });
            }
            return Ok(adverts);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error occurred while retrieving the list of adverts");
            return BadRequest(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpGet("GetAdvert/{id}")]
    public async Task<ActionResult<AdvertResponse>> GetAdvertById(string id)
    {
        try
        {
            var advert = await Mediator.Send(new GetAdvertsById { Id = id });
            advert.PhotoList = await Mediator.Send(new GetAdvertPhotoList{Id = id});
            
            var discount = await _discountClientGrpc.GetDiscount(id, advert.Price);
            if (discount.ExpiresAt != null)
            {
                advert.PriceWithDiscount = discount.CalculatedPrice;
                advert.DiscountExpirationTime = discount.ExpiresAt.ToDateTime();
            }

            advert.Seller = await Mediator.Send(new GetUserById { Id = advert.UserId });

            return Ok(advert);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while receiving the advert");
            return BadRequest(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpDelete("DeleteAdvert")]
    public async Task<ActionResult<string>> DeleteAdvert([FromQuery] DeleteAdvertCommand command)
    {
        try
        {
            await Mediator.Send(command);
            await _discountClientGrpc.DeleteAdvertDiscounts(command.Id);
            await Mediator.Send(new DeleteAdvertPhotoListsCommand { AdvertId = command.Id });
            
            _logger.LogInformation("Advert with id: {Id} deleted", command.Id);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while deleting the advert: {Id}", command.Id);
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<string>> UpdateAdvert(UpdateAdvertCommand command)
    {
        try
        {
            return Ok(await Mediator.Send(command));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while updating the advert: {Id}", command.Id);
            return BadRequest(e.Message);
        }
    }
}
