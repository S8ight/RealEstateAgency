using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.DAL.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        public Task<IEnumerable<Message>> GetMessagesOfChat(string chatId);
    }
}
