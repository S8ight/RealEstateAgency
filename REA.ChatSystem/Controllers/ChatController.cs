using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.Interfaces;

namespace REA.ChatSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly ILogger<ChatController> _logger;
    private readonly IChatService _chatService;
    public ChatController(IChatService chatService, ILogger<ChatController> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    [HttpGet("GetUserChats/{UserId}")]
    public async Task<IActionResult> GetUserChats(string userId)
    {
        try
        {
            var chats = await _chatService.GetUserChats(userId);

            if (chats != null && !chats.Any())
            {
                return Ok("User still has no chats.");
            } 
        
            return Ok(chats);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("GetChat/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var chat = await _chatService.GetByIdAsync(id);
        
            return Ok(chat);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }

    [HttpPost("CreateChat")]
    public async Task<IActionResult> CreateChat([FromBody] ChatRequest request)
    {
        try
        {
            var chatId = await _chatService.AddAsync(request);
            return Ok(chatId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("DeleteChat")]
    public async Task<IActionResult> DeleteChat(string id)
    {
        try
        {
            await _chatService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }
}
