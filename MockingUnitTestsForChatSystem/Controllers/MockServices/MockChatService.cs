using Moq;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;
using REA.ChatSystem.BLL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace MockingUnitTestsForChatSystem.Services;

public class MockChatService : Mock<IChatService>
{
    public MockChatService MockGetAllAsync(Task<IEnumerable<ChatResponse>> results)
    {
        Setup(service => service.GetAllAsync()).Returns(results);

        return this;
    }

    public MockChatService MockGetById(Task<ChatResponse> results)
    {
        Setup(service => service.GetByIdAsync(It.IsAny<string>())).Returns(results);

        return this;
    }
    
    public MockChatService MockGetById_Invalid()
    {
        Setup(service => service.GetByIdAsync(It.IsAny<string>()))
            .Throws(new ArgumentException("Chat with that Id was not found"));

        return this;
    }
    
    public MockChatService MockAdd(ChatRequest request, Task<string> newId)
    {
        Setup(service => service.AddAsync(request)).Returns(newId);
        
        return this;
    }
    
    public MockChatService MockAdd_Invalid()
    {
        Setup(service => service.AddAsync(null))
            .Throws(new ArgumentException("Not possible to create chat with this users"));
        
        return this;
    }
    
    public MockChatService MockDelete()
    {
        Setup(service => service.DeleteAsync(It.IsAny<string>()));
        
        return this;
    }
    
    public MockChatService MockDelete_Invalid()
    {
        Setup(service => service.DeleteAsync(It.IsAny<string>()))
            .Throws(new ArgumentException("Chat with that Id was not found"));
        
        return this;
    }
    
    public void VerifyGetAll(Times times)
    {
        Verify(x => x.GetAllAsync(), times);
    }

    public void VerifyGetById(Times times)
    {
        Verify(x => x.GetByIdAsync(It.IsAny<string>()), times);
    }
}