using MassTransit;
using Microsoft.AspNetCore.SignalR;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.Interfaces;

namespace REA.ChatSystem.BLL.Hubs;

public class ChatHub : Hub
{
    private readonly IMessageService _messageService;

    public ChatHub(IMessageService messageService)
    {
        _messageService = messageService;
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
            Console.WriteLine("Error while sending message: " + e.Message);
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
            Console.WriteLine("Error while updating message: " + e.Message);
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
            Console.WriteLine("Error while deleting message: " + e.Message);
            throw;
        }
    }

    public async Task JoinChatGroup(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }
}