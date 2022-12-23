using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MockingUnitTestsForChatSystem.Services;
using Moq;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;
using REA.ChatSystem.Controllers;

namespace MockingUnitTestsForChatSystem.Controllers;

public class ChatControllerTests
{
    [Fact]
    public void ChatController_GetAll_Valid()
    {
        var chats = new List<ChatResponse>()
        {
            new()
            {
                ChatId = "1",
                UserId = "1",
                RecieverId = "2"
            }
        };

        Task<IEnumerable<ChatResponse>> mockChats = Task.FromResult<IEnumerable<ChatResponse>>(chats);

        var mockChatService = new MockChatService().MockGetAllAsync(mockChats);

        var controller = new ChatController(mockChatService.Object);

        var result = controller.GetAll().Result;

        result.Should().BeOfType<OkObjectResult>();
        mockChatService.VerifyGetAll(Times.Once());
    }
    
    [Fact]
    public void ChatController_GetById_Valid()
    {
        var chat = new ChatResponse()
        {
            ChatId = "1",
            UserId = "3",
            RecieverId = "2"
        };

        Task<ChatResponse> mockChats = Task.FromResult(chat);

        var mockChatService = new MockChatService().MockGetById(mockChats);

        var controller = new ChatController(mockChatService.Object);

        var result = controller.GetById("1").Result;

        result.Should().BeOfType<OkObjectResult>();
        mockChatService.VerifyGetById(Times.Once());
    }
    
    [Fact]
    public void ChatController_GetById_Invalid()
    {
        var mockChatService = new MockChatService().MockGetById_Invalid();

        var controller = new ChatController(mockChatService.Object);

        var result = controller.GetById("0").Result;
        
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public void ChatController_Add_Valid()
    {
        var request = new ChatRequest()
        {
            ChatId = "1",
            UserId = "3",
            RecieverId = "2",
            Created = DateTime.Now
        };

        var mockChatService = new MockChatService().MockAdd(request, Task.FromResult("1"));

        var controller = new ChatController(mockChatService.Object);

        var result = controller.Add(request).Result;

        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public void ChatController_Add_Invalid()
    {
        var mockChatService = new MockChatService().MockAdd_Invalid();

        var controller = new ChatController(mockChatService.Object);

        var result = controller.Add(null!).Result;

        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public void ChatController_Delete_Valid()
    {
        var mockChatService = new MockChatService().MockDelete();

        var controller = new ChatController(mockChatService.Object);

        var result = controller.Delete("1").Result;

        result.Should().BeOfType<OkResult>();
    }
    
    [Fact]
    public void ChatController_Delete_Invalid()
    {
        var mockChatService = new MockChatService().MockDelete_Invalid();

        var controller = new ChatController(mockChatService.Object);

        var result = controller.Delete("1").Result;

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}