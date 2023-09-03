using Dapper;
using REA.ChatSystem.DAL.Context;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.DAL.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        private readonly IDatabaseConnectionFactory _dataContext;
        public ChatRepository(IDatabaseConnectionFactory dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task<IEnumerable<Chat>> GetUserChats(string userId)
        {
            return await _dataContext.GetConnection().QueryAsync<Chat>($"SELECT * FROM [Chat] WHERE UserId=@userId");
        }
    }
}
