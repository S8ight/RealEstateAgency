using Autofac.Extras.Moq;
using FluentAssertions;
using MockingUnitTestsForChatSystem.Repositories.Common;
using REA.ChatSystem.DAL.Context;
using REA.ChatSystem.DAL.Models;
using REA.ChatSystem.DAL.Repositories;

namespace MockingUnitTestsForChatSystem.Repositories;

public class ChatRepositoryTests
{
    [Fact]
    public async Task GetAllAsync()
    {
        var chats = GetChats();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(chats);
            
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new ChatRepository(connection);
            var result = await repository.GetAllAsync();
            
            result.Should().HaveCount(3);
        }
    }
    
    [Fact]
    public async Task GetAsync_Valid()
    {
        var chats = GetChats();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(chats);
            
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new ChatRepository(connection);
            var result = await repository.GetAsync(chats.First().ChatId);
            
            result.Should().NotBeNull();
            result.Should().BeOfType<Chat>();
        }
    }
    
    [Fact]
    public async Task GetAsync_Invalid()
    {
        var chats = GetChats();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(chats);
            
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new ChatRepository(connection);
            
            Func<Task> act = async () => await repository.GetAsync("0");
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
    
    [Fact]
    public async Task AddAsync_Valid()
    {
        var chats = GetChats();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(chats);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new ChatRepository(connection);
            await repository.AddAsync(getNewChat());
            var result = await repository.GetAllAsync();
                
            result.Should().HaveCount(4);
        }
    }
    
    [Fact]
    public async Task AddAsync_Invalid()
    {
        var chats = GetChats();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(chats);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new ChatRepository(connection);

            Func<Task> act = async () => await repository.AddAsync(null);
            await act.Should().ThrowAsync<System.Data.SQLite.SQLiteException>();
        }
    }
    
    [Fact]
    public async Task ReplaceAsync_Valid()
    {
        var chats = GetChats();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(chats);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new ChatRepository(connection);
            var chat = chats.First();
            chat.RecieverId = Guid.NewGuid().ToString();
            
            await repository.ReplaceAsync(chat);
            var result = await repository.GetAsync(chats.First().ChatId);

            result.RecieverId.Should().Be(chat.RecieverId);
        }
    }
    
    [Fact]
    public async Task ReplaceAsync_Invalid()
    {
        var chats = GetChats();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(chats);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new ChatRepository(connection);

            Func<Task> act = async () => await repository.ReplaceAsync(null);
            await act.Should().ThrowAsync<System.Data.SQLite.SQLiteException>();
        }
    }
    
    [Fact]
    public async Task DeleteAsync_Valid()
    {
        var chats = GetChats();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(chats);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new ChatRepository(connection);

            await repository.DeleteAsync(chats.First().ChatId);
            var result = await repository.GetAllAsync();
                
            result.Should().HaveCount(2);
        }
    }
    
    [Fact]
    public async Task DeleteAsync_Invalid()
    {
        var chats = GetChats();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(chats);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new ChatRepository(connection);

            Func<Task> act = async () => await repository.DeleteAsync(null);
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
    
    [Fact]
    public async Task AddRangeAsync()
    {
        var chats = GetChats();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(chats);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new ChatRepository(connection);

            await repository.AddRangeAsync(GetChats());
            var result = await repository.GetAllAsync();
                
            result.Should().HaveCount(6);
        }
    }
    
    


    public Chat getNewChat()
    {
        return new()
        {
            ChatId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString(),
            RecieverId = Guid.NewGuid().ToString(),
            Created = DateTime.Now,
        };
    }

    public IEnumerable<Chat> GetChats()
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
}