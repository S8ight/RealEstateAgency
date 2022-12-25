using AuthorizationService.DAL.Entities;

namespace AuthorizationService.DAL.Data.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(string id);
    Task<User?> GetByEmail(string email);
    Task<User?> GetByToken(string token);
    Task<User> Create(User user);
    Task Delete(string id);
}