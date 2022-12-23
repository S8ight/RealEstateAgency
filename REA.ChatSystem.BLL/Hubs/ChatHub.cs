using Microsoft.AspNetCore.SignalR;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.BLL.Hubs;

public class ChatHub : Hub
{
    public async Task Send(Message message)
    {
        await Clients.All.SendAsync("receiveMessage", message);
    }
    
}