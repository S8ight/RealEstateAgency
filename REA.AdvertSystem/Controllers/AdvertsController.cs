using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.Interfaces.Services;

namespace REA.AdvertSystem.Controllers;

[ApiController]
[Route("api/v1")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdvertsController : ControllerBase
{
    private readonly IAdvertService _advertService;
    
    public AdvertsController(IAdvertService advertService)
    {
        _advertService = advertService;
    }
    
    [AllowAnonymous]
    [HttpGet("adverts")]
    public async Task<IActionResult> GetAdverts([FromQuery] AdvertsFilterRequest request)
    {
        var result = await _advertService.GetFilteredAdvertsAsync(request);
        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpGet("adverts/user")]
    public async Task<IActionResult> GetUserAdverts([FromQuery] UserSaveListRequest request)
    {
        var result = await _advertService.GetUserAdvertsAsync(request);
        return Ok(result);
    }
    
        
    [AllowAnonymous]
    [HttpGet("adverts/search")]
    public async Task<IActionResult> SearchAdverts([FromQuery] SearchAdvertsRequest request)
    {
        var result = await _advertService.SearchAdvertsAsync(request);
        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpGet("adverts/{id}")]
    public async Task<IActionResult> GetAdvert(string id)
    {
        var result = await _advertService.GetAdvertByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPost("adverts")]
    public async Task<IActionResult> CreateAdvert(AdvertRequest request)
    {
        await _advertService.AddAdvertAsync(request);
        return Ok();
    }
    
    [HttpPut("adverts/{id}")]
    public async Task<IActionResult> UpdateAdvert(string id, AdvertRequest request)
    {
        await _advertService.UpdateAdvertAsync(id, request);
        return Ok();
    }
    
    [HttpDelete("adverts/{id}")]
    public async Task<IActionResult> DeleteAdvert(string id)
    {
        await _advertService.DeleteAdvertAsync(id);
        return Ok();
    }
}