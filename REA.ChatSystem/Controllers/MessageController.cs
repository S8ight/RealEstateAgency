using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.Interfaces;

namespace REA.ChatSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ILogger<MessageController> _logger;
    public MessageController(IMessageService messageService, ILogger<MessageController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var data = await _messageService.GetAllAsync();
            return Ok(data);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [Authorize]
    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetAllChatMessages(string chatId)
    {
        try
        {
            var data = await _messageService.GetAllMessagesForChatAsync(chatId);
            return Ok(data);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var data = await _messageService.GetAsync(id);
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
    public async Task<IActionResult> Add([FromBody] MessageRequest request)
    {
        try
        {
            var data = await _messageService.AddAsync(request);
            return Ok(data);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] MessageRequest request)
    {
        try
        {
            await _messageService.UpdateAsync(request);
            return Ok();
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
            await _messageService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}