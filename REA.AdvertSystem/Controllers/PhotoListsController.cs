using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.Interfaces.Services;

namespace REA.AdvertSystem.Controllers;

[ApiController]
[Route("api/v1")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PhotoListsController : ControllerBase
{
    private readonly IPhotoListService _photoListService;
    
    public PhotoListsController(IPhotoListService photoListService)
    {
        _photoListService = photoListService;
    }
    
    [HttpPost("photo-lists")]
    [RequestSizeLimit(80000000)]
    public async Task<IActionResult> UploadImages([FromForm] UploadPhotoListRequest request)
    {
        await _photoListService.UploadImagesAsync(request);
        return Ok();
    }
    
    [HttpDelete("photo-lists/{id}")]
    public async Task<IActionResult> DeletePhoto(string id)
    {
        await _photoListService.DeletePhotoAsync(id);
        return Ok();
    }
}