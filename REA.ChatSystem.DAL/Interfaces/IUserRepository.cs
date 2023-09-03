using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.DAL.Interfaces
{
    public interface IUserRepository
    {
        public Task<string> AddAsync(User t);
        public Task DeleteAsync(string id);
        public Task<User> GetAsync(string id);
        public Task<Task> ReplaceAsync(User t);

    }
}
