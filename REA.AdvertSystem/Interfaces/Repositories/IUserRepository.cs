using REA.AdvertSystem.DataAccess.Entities;

namespace REA.AdvertSystem.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(string id);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(string id);
}