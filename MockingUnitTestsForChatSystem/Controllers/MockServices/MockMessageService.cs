using Moq;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;
using REA.ChatSystem.BLL.Interfaces;

namespace MockingUnitTestsForChatSystem.Controllers.MockServices;

public class MockMessageService: Mock<IMessageService>
{
    public MockMessageService MockGetAllAsync(Task<IEnumerable<MessageResponse>> results)
    {
        Setup(service => service.GetAllAsync()).Returns(results);
        
        return this;
    }
    
    public MockMessageService MockGetAllMessagesForChatAsync(Task<IEnumerable<MessageResponse>> results)
    {
        Setup(service => service.GetAllMessagesForChatAsync(It.IsAny<string>()))
            .Returns(results);
        
        return this;
    }
    
    public MockMessageService MockGetAllMessagesForChatAsync_Invalid()
    {
        Setup(service => service.GetAllMessagesForChatAsync(It.IsAny<string>()))
            .Throws(new ArgumentException("Chat with that Id not found"));
        
        return this;
    }
    
    public MockMessageService MockGetAsync(Task<MessageResponse> results)
    {
        Setup(service => service.GetAsync(It.IsAny<string>()))
            .Returns(results);
        
        return this;
    }
    
    public MockMessageService MockGetAsync_Invalid()
    {
        Setup(service => service.GetAsync(It.IsAny<string>()))
            .Throws(new ArgumentException("Message with that Id not found"));
        
        return this;
    }
    
    public MockMessageService MockUpdateAsync(MessageRequest request)
    {
        Setup(service => service.UpdateAsync(request));
        
        return this;
    }
    
    public MockMessageService MockUpdateAsync_Invalid()
    {
        Setup(service => service.UpdateAsync(null))
            .Throws(new ArgumentException("Message with that Id not found"));
        
        return this;
    }
    
    public MockMessageService MockAddAsync(MessageRequest request, Task<string> newId)
    {
        Setup(service => service.AddAsync(request)).Returns(newId);
        
        return this;
    }
    
    public MockMessageService MockAddAsync_Invalid()
    {
        Setup(service => service.AddAsync(null))
            .Throws(new ArgumentException("Message with that Id not found"));
        
        return this;
    }
    
    public MockMessageService MockDeleteAsync()
    {
        Setup(service => service.DeleteAsync(It.IsAny<string>()));
        
        return this;
    }
    
    public MockMessageService MockDeleteAsync_Invalid()
    {
        Setup(service => service.DeleteAsync(It.IsAny<string>()))
            .Throws(new ArgumentException("Message with that Id not found"));;
        
        return this;
    }
    
    public void VerifyGetAll(Times times)
    {
        Verify(x => x.GetAllAsync(), times);
    }

    public void VerifyGetById(Times times)
    {
        Verify(x => x.GetAsync(It.IsAny<string>()), times);
    }
}