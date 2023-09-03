using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.Interfaces;

namespace REA.ChatSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ILogger<MessageController> _logger;
    public MessageController(IMessageService messageService, ILogger<MessageController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    [HttpGet("GetChatMessages/{chatId}")]
    public async Task<IActionResult> GetChatMessages(string chatId)
    {
        try
        {
            var messages = await _messageService.GetAllMessagesForChatAsync(chatId);
            return Ok(messages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpGet("GetMessage/{messageId}")]
    public async Task<IActionResult> GetMessageById(string messageId)
    {
        try
        {
            var message = await _messageService.GetAsync(messageId);
            return Ok(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("CreateMessage")]
    public async Task<IActionResult> CreateMessage([FromBody] MessageRequest request)
    {
        try
        {
            await _messageService.AddAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("UpdateMessage")]
    public async Task<IActionResult> UpdateMessage([FromBody] MessageUpdateRequest request)
    {
        try
        {
            await _messageService.UpdateAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("DeleteMessage")]
    public async Task<IActionResult> DeleteMessage([FromBody] MessageDeleteRequest request)
    {
        try
        {
            await _messageService.DeleteAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the request");
            return BadRequest(e.Message);
        }
    }
}