using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.DAL.Interfaces
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        Task<IEnumerable<Chat>> GetUserChats(string userId);
    }
}
