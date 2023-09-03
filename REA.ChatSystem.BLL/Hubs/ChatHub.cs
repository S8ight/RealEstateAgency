using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.Interfaces;

namespace REA.ChatSystem.BLL.Hubs;

public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IMessageService messageService, ILogger<ChatHub> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    public async Task SendMessage(MessageRequest messageRequest)
    {
        try
        {
            var messageId = await _messageService.AddAsync(messageRequest);

            await Clients.Group(messageRequest.ChatId).SendAsync("ReceiveMessage",
                messageId, messageRequest.MessageBody, messageRequest.Created.ToString("HH:mm"), messageRequest.SenderId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while sending message");
            throw;
        }

    }
    
    public async Task UpdateMessage(MessageUpdateRequest messageRequest)
    {
        try
        {
            await _messageService.UpdateAsync(messageRequest);
        
            await Clients.Group(messageRequest.ChatId).SendAsync("MessageUpdated",
                messageRequest.MessageId, messageRequest.MessageBody);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating message");
            throw;
        }
    }
    
    public async Task DeleteMessage(MessageDeleteRequest messageRequest)
    {
        try
        {
            await _messageService.DeleteAsync(messageRequest);
        
            await Clients.Group(messageRequest.ChatId).SendAsync("MessageDelete",
                messageRequest.MessageId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting message");
            throw;
        }
    }

    public async Task JoinChatGroup(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }
}