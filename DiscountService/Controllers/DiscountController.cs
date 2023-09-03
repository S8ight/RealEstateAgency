using AutoMapper;
using DiscountService.DataAccess.Interfaces;
using DiscountService.DTO;
using DiscountService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DiscountService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DiscountController> _logger;

    public DiscountController(IDiscountRepository discountRepository, IMapper mapper, ILogger<DiscountController> logger)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpGet("GetAdvertDiscounts/{advertId}")]
    public async Task<IActionResult> GetAdvertDiscounts(string advertId)
    {
        try
        {
            var discountList = await _discountRepository.GetAdvertDiscounts(advertId);
        
            return Ok(discountList);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpGet("GetAdvertCurrentDiscount/{advertId}")]
    public async Task<IActionResult> GetAdvertCurrentDiscount(string advertId)
    {
        try
        {
            var discount = await _discountRepository.GetAdvertCurrentDiscount(advertId);
        
            return Ok(discount);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpGet("GetDiscountById/{id}")]
    public async Task<IActionResult> GetDiscountById(string id)
    {
        try
        {
            var discount = await _discountRepository.GetDiscountById(id);
        
            return Ok(discount);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpPost("AddDiscount")]
    public async Task<IActionResult> AddDiscount([FromBody] DiscountRequest request)
    {
        try
        {
            var discount = _mapper.Map<DiscountRequest,Discount>(request);
            await _discountRepository.AddDiscount(discount);
            
            return Ok("New discount created");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("DeleteDiscount/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _discountRepository.DeleteDiscount(id);
            return Ok("Discount deleted");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}