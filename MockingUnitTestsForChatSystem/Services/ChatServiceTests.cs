/*using Autofac.Extras.Moq;
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

public class ChatServiceTests
{
    private IMapper _mapper = new MapperConfiguration(x =>
                                x.AddProfile(new MapperCfg())).CreateMapper();

    [Fact]
    public async Task GetAllAsync()
    {
        var chats = GetChats();
        using (var mock = AutoMock.GetLoose())
        {
            mock.Mock<IUnitOfWork>()
                .Setup(unit => unit.ChatRepository.GetAllAsync())
                .Returns(Task.FromResult(chats));
            
            var unit = mock.Create<IUnitOfWork>();
            var chatService = new ChatService(unit, _mapper);
            var result = await chatService.GetAllAsync();
            
            result.Should().HaveCount(3);
        }
    }
    
    [Fact]
    public async Task GetByIdAsync()
    {
        var chat = GetChats().First();
        using (var mock = AutoMock.GetLoose())
        {
            mock.Mock<IUnitOfWork>()
                .Setup(unit => unit.ChatRepository.GetAsync(chat.ChatId))
                .Returns(Task.FromResult(chat));
            
            var unit = mock.Create<IUnitOfWork>();
            var chatService = new ChatService(unit, _mapper);
            var result = await chatService.GetByIdAsync(chat.ChatId);

            result.Should().NotBeNull();
            result.Should().BeOfType<ChatResponse>();
        }
    }
    
    [Fact]
    public async Task AddAsync()
    {
        var request = new ChatRequest()
        {
            ChatId = Guid.NewGuid().ToString(),
            Created = DateTime.Now,
            RecieverId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString()
        };
        var chat = _mapper.Map<Chat>(request);
        
        using (var mock = AutoMock.GetLoose())
        {
            mock.Mock<IUnitOfWork>()
                .Setup(unit => unit.ChatRepository.AddAsync(It.Is<Chat>(c => c.ChatId == request.ChatId)))
                .Returns(Task.FromResult(chat.ChatId));
            var unit = mock.Create<IUnitOfWork>();
            var chatService = new ChatService(unit, _mapper);
            var result = await chatService.AddAsync(request);
            
            result.Should().NotBeNull();
            result.Should().Be(request.ChatId);
        }
    }
    
    [Fact]
    public async Task DeleteAsync()
    {
        using (var mock = AutoMock.GetLoose())
        {
            mock.Mock<IUnitOfWork>()
                .Setup(unit => unit.ChatRepository.DeleteAsync("1"));
            
            var unit = mock.Create<IUnitOfWork>();
            var chatService = new ChatService(unit, _mapper);
            await chatService.DeleteAsync("1");
            
            mock.Mock<IUnitOfWork>().Verify(x => 
                                        x.ChatRepository.DeleteAsync("1"), Times.Once);
        }
    }

    private IEnumerable<Chat> GetChats()
    {
        List<Chat> chats = new()
        {
            new()
            {
                ChatId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                RecieverId = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            },
            new()
            {
                ChatId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                RecieverId = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            },
            new()
            {
                ChatId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                RecieverId = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            }
        };
        return chats;
    }
    
}*/