using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.Interfaces.Services;

namespace REA.AdvertSystem.Controllers;

[ApiController]
[Route("api/v1")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SaveListsController : ControllerBase
{
    private readonly ISaveListService _saveListService;
    
    public SaveListsController(ISaveListService saveListService)
    {
        _saveListService = saveListService;
    }
    
    [HttpGet("save-lists")]
    public async Task<IActionResult> GetUserSaveLists([FromQuery] UserSaveListRequest request)
    {
        var result = await _saveListService.GetUserSaveList(request);
        return Ok(result);
    }
    
    [HttpPost("save-lists")]
    public async Task<IActionResult> CreateSaveList(SaveListRequest request)
    {
        await _saveListService.AddSaveListAsync(request);
        return Ok();
    }
    
    [HttpDelete("save-lists/{userId}/{advertId}")]
    public async Task<IActionResult> DeleteSaveList(string userId, string advertId)
    {
        await _saveListService.DeleteSaveListAsync(userId, advertId);
        return Ok();
    }
}