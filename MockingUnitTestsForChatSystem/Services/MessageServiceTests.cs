using Autofac.Extras.Moq;
using AutoMapper;
using FluentAssertions;
using Moq;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;
using REA.ChatSystem.BLL.Mapper;
using REA.ChatSystem.BLL.Services;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace MockingUnitTestsForChatSystem.Services;

public class MessageServiceTests
{
    private IMapper _mapper = new MapperConfiguration(x =>
                                x.AddProfile(new MapperCfg())).CreateMapper();

    [Fact]
    public async Task GetAllAsync()
    {
        var messages = getMessages();
        using (var mock = AutoMock.GetLoose())
        {
            mock.Mock<IUnitOfWork>()
                .Setup(unit => unit.MessageRepository.GetAllAsync())
                .Returns(Task.FromResult(messages));
            
            var unit = mock.Create<IUnitOfWork>();
            var messageService = new MessageService(unit, _mapper);
            var result = await messageService.GetAllAsync();
            
            result.Should().HaveCount(3);
        }
    }
    
    [Fact]
    public async Task GetAllMessagesForChatAsync()
    {
        var messages = getMessages();
        using (var mock = AutoMock.GetLoose())
        {
            mock.Mock<IUnitOfWork>()
                .Setup(unit => unit.MessageRepository.GetMessagesOfChat("3"))
                .Returns(Task.FromResult(messages));
            
            var unit = mock.Create<IUnitOfWork>();
            var messageService = new MessageService(unit, _mapper);
            var result = await messageService.GetAllMessagesForChatAsync("3");
            
            result.Should().HaveCount(3);
        }
    }
    
    [Fact]
    public async Task GetAsync()
    {
        var message = getMessages().First();
        using (var mock = AutoMock.GetLoose())
        {
            mock.Mock<IUnitOfWork>()
                .Setup(unit => unit.MessageRepository.GetAsync(message.MessageId))
                .Returns(Task.FromResult(message));
            
            var unit = mock.Create<IUnitOfWork>();
            var messageService = new MessageService(unit, _mapper);
            var result = await messageService.GetAsync(message.MessageId);
            
            result.Should().NotBeNull();
            result.Should().BeOfType<MessageResponse>();
        }
    }
    
    [Fact]
    public async Task AddAsync()
    {
        var request = new MessageRequest()
        {
            MessageId = Guid.NewGuid().ToString(),
            Created = DateTime.Now,
            ChatId = Guid.NewGuid().ToString(),
            SenderId = Guid.NewGuid().ToString(),
            RecieverId = Guid.NewGuid().ToString(),
            MessageBody = "Some user text"
        };
        var message = _mapper.Map<Message>(request);
        
        using (var mock = AutoMock.GetLoose())
        {
            mock.Mock<IUnitOfWork>()
                .Setup(unit => unit.MessageRepository.AddAsync(It.Is<Message>(c =>
                    c.MessageId == request.MessageId)))
                        .Returns(Task.FromResult(message.MessageId));
            
            var unit = mock.Create<IUnitOfWork>();
            var messageService = new MessageService(unit, _mapper);
            var result = await messageService.AddAsync(request);
            
            result.Should().NotBeNull();
            result.Should().Be(request.MessageId);
        }
    }
    
    [Fact]
    public async Task UpdateAsync()
    {
        var request = new MessageRequest()
        {
            MessageId = Guid.NewGuid().ToString(),
            Created = DateTime.Now,
            ChatId = Guid.NewGuid().ToString(),
            SenderId = Guid.NewGuid().ToString(),
            RecieverId = Guid.NewGuid().ToString(),
            MessageBody = "Some user text"
        };
        var message = _mapper.Map<Message>(request);
        
        using (var mock = AutoMock.GetLoose())
        {
            mock.Mock<IUnitOfWork>()
                .Setup(unit =>
                    unit.MessageRepository.ReplaceAsync(It.Is<Message>(c => 
                        c.MessageId == request.MessageId)));
            
            var unit = mock.Create<IUnitOfWork>();
            var messageService = new MessageService(unit, _mapper);
            await messageService.UpdateAsync(request);
            
            mock.Mock<IUnitOfWork>().Verify(x => 
                    x.MessageRepository.ReplaceAsync(It.Is<Message>(m =>
                        m.MessageId == request.MessageId)), Times.Once);
        }
    }
    
    [Fact]
    public async Task DeleteAsync()
    {
        using (var mock = AutoMock.GetLoose())
        {
            mock.Mock<IUnitOfWork>()
                .Setup(unit => unit.MessageRepository.DeleteAsync("1"));
            
            var unit = mock.Create<IUnitOfWork>();
            var messageService = new MessageService(unit, _mapper);
            await messageService.DeleteAsync("1");
            
            mock.Mock<IUnitOfWork>().Verify(x => 
                                        x.MessageRepository.DeleteAsync("1"), Times.Once);
        }
    }

    private IEnumerable<Message> getMessages()
    {
        List<Message> messages = new List<Message>()
        {
            new()
            {
                MessageId = Guid.NewGuid().ToString(),
                ChatId = Guid.NewGuid().ToString(),
                SenderId = Guid.NewGuid().ToString(),
                RecieverId = Guid.NewGuid().ToString(),
                MessageBody = "Message body",
                Created = DateTime.Now,
                Checked = false
            },
            new()
            {
                MessageId = Guid.NewGuid().ToString(),
                ChatId = Guid.NewGuid().ToString(),
                SenderId = Guid.NewGuid().ToString(),
                RecieverId = Guid.NewGuid().ToString(),
                MessageBody = "Message text",
                Created = DateTime.Now,
                Checked = false
            },
            new()
            {
                MessageId = Guid.NewGuid().ToString(),
                ChatId = Guid.NewGuid().ToString(),
                SenderId = Guid.NewGuid().ToString(),
                RecieverId = Guid.NewGuid().ToString(),
                MessageBody = "Some message text",
                Created = DateTime.Now,
                Checked = false
            }
        };
        return messages;
    }
}