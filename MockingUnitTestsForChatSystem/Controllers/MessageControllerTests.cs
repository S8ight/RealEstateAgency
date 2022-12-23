using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MockingUnitTestsForChatSystem.Controllers.MockServices;
using Moq;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;
using REA.ChatSystem.Controllers;

namespace MockingUnitTestsForChatSystem.Controllers;

public class MessageControllerTests
{
    [Fact]
    public void MessageController_GetAll_Valid()
    {
        var messages = new List<MessageResponse>()
        {
            new()
            {
                MessageId = "1",
                SenderId = "1",
                MessageBody = "Message text",
                Created = DateTime.Now
            }
        };

        Task<IEnumerable<MessageResponse>> mockMessages = Task.FromResult<IEnumerable<MessageResponse>>(messages);

        var mockMessageService = new MockMessageService().MockGetAllAsync(mockMessages);

        var controller = new MessageController(mockMessageService.Object);

        var result = controller.GetAll().Result;

        result.Should().BeOfType<OkObjectResult>();
        mockMessageService.VerifyGetAll(Times.Once());
    }
    
    [Fact]
    public void MessageController_GetAllMessagesForChatAsync_Valid()
    {
        var messages = new List<MessageResponse>()
        {
            new()
            {
                MessageId = "1",
                SenderId = "1",
                MessageBody = "Message text",
                Created = DateTime.Now
            }
        };

        Task<IEnumerable<MessageResponse>> mockMessages = Task.FromResult<IEnumerable<MessageResponse>>(messages);

        var mockMessageService = new MockMessageService().MockGetAllMessagesForChatAsync(mockMessages);

        var controller = new MessageController(mockMessageService.Object);

        var result = controller.GetAllChatMessages("1").Result;

        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public void MessageController_GetAllMessagesForChatAsync_Invalid()
    {
        var mockMessageService = new MockMessageService().MockGetAllMessagesForChatAsync_Invalid();

        var controller = new MessageController(mockMessageService.Object);

        var result = controller.GetAllChatMessages("0").Result;

        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public void MessageController_GetById_Valid()
    {
        var message = new MessageResponse()
        {
            MessageId = "1",
            SenderId = "1",
            MessageBody = "Message text",
            Created = DateTime.Now
        };

        Task<MessageResponse> mockmessages = Task.FromResult(message);

        var mockMessageService = new MockMessageService().MockGetAsync(mockmessages);

        var controller = new MessageController(mockMessageService.Object);

        var result = controller.GetById("1").Result;

        result.Should().BeOfType<OkObjectResult>();
        mockMessageService.VerifyGetById(Times.Once());
    }
    
    [Fact]
    public void MessageController_GetById_Invalid()
    {
        var mockMessageService = new MockMessageService().MockGetAsync_Invalid();

        var controller = new MessageController(mockMessageService.Object);

        var result = controller.GetById("0").Result;

        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public void MessageController_Add_Valid()
    {
        var message = new MessageRequest()
        {
            MessageId = "1",
            ChatId = "3",
            SenderId = "1",
            RecieverId = "2",
            MessageBody = "Message text",
            Created = DateTime.Now
        };

        var mockMessageService = new MockMessageService().MockAddAsync(message, Task.FromResult("1"));

        var controller = new MessageController(mockMessageService.Object);

        var result = controller.Add(message).Result;

        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public void MessageController_Add_Invalid()
    {
        var mockMessageService = new MockMessageService().MockAddAsync_Invalid();

        var controller = new MessageController(mockMessageService.Object);

        var result = controller.Add(null!).Result;

        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public void MessageController_Update_Valid()
    {
        var message = new MessageRequest()
        {
            MessageId = "1",
            ChatId = "3",
            SenderId = "1",
            RecieverId = "2",
            MessageBody = "Message text",
            Created = DateTime.Now
        };

        var mockMessageService = new MockMessageService().MockUpdateAsync(message);

        var controller = new MessageController(mockMessageService.Object);

        var result = controller.Update(message).Result;

        result.Should().BeOfType<OkResult>();
    }
    
    [Fact]
    public void MessageController_Update_Invalid()
    {
        var mockMessageService = new MockMessageService().MockUpdateAsync_Invalid();

        var controller = new MessageController(mockMessageService.Object);

        var result = controller.Update(null!).Result;

        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public void MessageController_Delete_Valid()
    {
        var mockMessageService = new MockMessageService().MockDeleteAsync();

        var controller = new MessageController(mockMessageService.Object);

        var result = controller.Delete("1").Result;

        result.Should().BeOfType<OkResult>();
    }
    
    [Fact]
    public void MessageController_Delete_Invalid()
    {
        var mockMessageService = new MockMessageService().MockDeleteAsync_Invalid();

        var controller = new MessageController(mockMessageService.Object);

        var result = controller.Delete("1").Result;

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}