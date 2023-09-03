/*using Autofac.Extras.Moq;
using FluentAssertions;
using MockingUnitTestsForChatSystem.Repositories.Common;
using REA.ChatSystem.DAL.Context;
using REA.ChatSystem.DAL.Models;
using REA.ChatSystem.DAL.Repositories;

namespace MockingUnitTestsForChatSystem.Repositories;

public class MessageRepositoryTests
{
    [Fact]
    public async Task GetAllAsync()
    {
        var messages = GetMessages();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(messages);
            
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new MessageRepository(connection);
            var result = await repository.GetAllAsync();
            
            result.Should().HaveCount(3);
        }
    }
    
    [Fact]
    public async Task GetAsync_Valid()
    {
        var messages = GetMessages();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(messages);
            
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new MessageRepository(connection);
            var result = await repository.GetAsync(messages.First().MessageId);
            
            result.Should().NotBeNull();
            result.Should().BeOfType<Message>();
        }
    }
    
    [Fact]
    public async Task GetAsync_Invalid()
    {
        var messages = GetMessages();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(messages);
            
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new MessageRepository(connection);
            
            Func<Task> act = async () => await repository.GetAsync("0");
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
    
    [Fact]
    public async Task AddAsync_Valid()
    {
        var messages = GetMessages();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(messages);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new MessageRepository(connection);
            await repository.AddAsync(getNewMessage());
            var result = await repository.GetAllAsync();
                
            result.Should().HaveCount(4);
        }
    }
    
    [Fact]
    public async Task AddAsync_Invalid()
    {
        var messages = GetMessages();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(messages);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new MessageRepository(connection);

            Func<Task> act = async () => await repository.AddAsync(null);
            await act.Should().ThrowAsync<System.Data.SQLite.SQLiteException>();
        }
    }
    
    [Fact]
    public async Task ReplaceAsync_Valid()
    {
        var messages = GetMessages();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(messages);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new MessageRepository(connection);
            var message = messages.First();
            message.MessageBody = "New text fot message";
            
            await repository.ReplaceAsync(message);
            var result = await repository.GetAsync(messages.First().MessageId);

            result.MessageBody.Should().Be(message.MessageBody);
        }
    }
    
    [Fact]
    public async Task ReplaceAsync_Invalid()
    {
        var messages = GetMessages();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(messages);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new MessageRepository(connection);

            Func<Task> act = async () => await repository.ReplaceAsync(null);
            await act.Should().ThrowAsync<System.Data.SQLite.SQLiteException>();
        }
    }
    
    [Fact]
    public async Task DeleteAsync_Valid()
    {
        var messages = GetMessages();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(messages);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new MessageRepository(connection);

            await repository.DeleteAsync(messages.First().MessageId);
            var result = await repository.GetAllAsync();
                
            result.Should().HaveCount(2);
        }
    }
    
    [Fact]
    public async Task DeleteAsync_Invalid()
    {
        var messages = GetMessages();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(messages);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new MessageRepository(connection);

            Func<Task> act = async () => await repository.DeleteAsync(null);
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
    
    [Fact]
    public async Task AddRangeAsync()
    {
        var messages = GetMessages();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(messages);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new MessageRepository(connection);

            await repository.AddRangeAsync(GetMessages());
            var result = await repository.GetAllAsync();
                
            result.Should().HaveCount(6);
        }
    }
    
    [Fact]
    public async Task GetMessagesOfChat()
    {
        var messages = GetMessages();
        using (var mock = AutoMock.GetLoose())
        {
            var db = new InMemoryDatabase();
            db.Insert(messages);
                
            mock.Mock<IDatabaseConnectionFactory>().Setup(c => c.GetConnection())
                .Returns(db.OpenConnection());

            var connection = mock.Create<IDatabaseConnectionFactory>();
            var repository = new MessageRepository(connection);

            var result = await repository.GetAllAsync();

            result.Should().HaveCount(3);
        }
    }
    
    


    public Message getNewMessage()
    {
        return new()
        {
            MessageId = Guid.NewGuid().ToString(),
            ChatId = Guid.NewGuid().ToString(),
            SenderId = Guid.NewGuid().ToString(),
            RecieverId = Guid.NewGuid().ToString(),
            MessageBody = "Message body text",
            Checked = false,
            Created = DateTime.Now
        };
    }

    public IEnumerable<Message> GetMessages()
    {
        List<Message> messages = new()
        {
            new()
            {
                MessageId = Guid.NewGuid().ToString(),
                ChatId = Guid.NewGuid().ToString(),
                SenderId = Guid.NewGuid().ToString(),
                RecieverId = Guid.NewGuid().ToString(),
                MessageBody = "Message body text",
                Checked = false,
                Created = DateTime.Now
            },
            new()
            {
                MessageId = Guid.NewGuid().ToString(),
                ChatId = Guid.NewGuid().ToString(),
                SenderId = Guid.NewGuid().ToString(),
                RecieverId = Guid.NewGuid().ToString(),
                MessageBody = "Message body text",
                Checked = false,
                Created = DateTime.Now
            },
            new()
            {
                MessageId = Guid.NewGuid().ToString(),
                ChatId = Guid.NewGuid().ToString(),
                SenderId = Guid.NewGuid().ToString(),
                RecieverId = Guid.NewGuid().ToString(),
                MessageBody = "Message body text",
                Checked = false,
                Created = DateTime.Now
            }
        };
        return messages;
    }
}*/