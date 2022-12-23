using Microsoft.AspNetCore.Mvc;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.Interfaces;

namespace REA.ChatSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
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
            return BadRequest(e.Message);
        }
    }

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
            return BadRequest(e.Message);
        }
    }
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
            return BadRequest(e.Message);
        }
    }
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
            return BadRequest(e.Message);
        }
    }
}