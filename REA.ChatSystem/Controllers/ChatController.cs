using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.Interfaces;

namespace REA.ChatSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly ILogger<ChatController> _logger;
    private readonly IChatService _chatService;
    public ChatController(IChatService chatService, ILogger<ChatController> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _chatService.GetAllAsync();

        return Ok(data);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var data = await _chatService.GetByIdAsync(id);
        
            return Ok(data);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] ChatRequest request)
    {
        try
        {
            var data = await _chatService.AddAsync(request);
            return Ok(data);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _chatService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}
