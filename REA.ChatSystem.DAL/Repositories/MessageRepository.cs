using Dapper;
using REA.ChatSystem.DAL.Context;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.DAL.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        private readonly IDatabaseConnectionFactory _dataContext;
        
        public MessageRepository(IDatabaseConnectionFactory dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<Message>> GetMessagesOfChat(string chatId)
        {
            return await _dataContext.GetConnection().QueryAsync<Message>($"SELECT * FROM [Message] WHERE ChatId=@chatId");
        }
    }
}
