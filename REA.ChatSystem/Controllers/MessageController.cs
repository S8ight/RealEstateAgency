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
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] MessageRequest request)
    {
        try
        {
            await _messageService.AddAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] MessageUpdateRequest request)
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
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] MessageDeleteRequest request)
    {
        try
        {
            await _messageService.DeleteAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}