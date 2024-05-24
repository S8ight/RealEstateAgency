using REA.AdvertSystem.DataAccess.Entities;
using REA.AdvertSystem.Interfaces.Repositories;

namespace REA.AdvertSystem.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AdvertDbContext _context;
    
    public UserRepository(AdvertDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteUserAsync(string id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
        {
            throw new ArgumentNullException($"User with {id} not found");
        }
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}