using AutoMapper;
using DiscountService.DTO;
using DiscountService.Entities;
using DiscountService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace DiscountService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public DiscountController(IDiscountRepository discountRepository, IMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var data = await _discountRepository.GetDiscount(id);
        
            return Ok(data);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] DiscountRequest request)
    {
        try
        {
            var discount = _mapper.Map<DiscountRequest,Discount>(request);
            discount.Created = DateTime.Now;
            await _discountRepository.AddDiscount(discount);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _discountRepository.DeleteDiscount(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}