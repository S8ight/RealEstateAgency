using AuthorizationService.DAL.Context;
using AuthorizationService.DAL.Data.Interfaces;
using AuthorizationService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.DAL.Data.Repositories;

public class UserRepository : IUserRepository
{
    protected readonly AuthorizationContext _context;
    
    protected readonly DbSet<User> _user;
    
    public UserRepository(AuthorizationContext context)
    {
        _context = context;
        _user = _context.Set<User>();
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _user.ToListAsync();
    }

    public async Task<User> GetById(string id)
    {
        return await _user.FindAsync(id)
               ?? throw new Exception(
                   $"User with id {id} not found");
    }
    
    public async Task<User?> GetByEmail(string email)
    {
        return await _user.SingleOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<User?> GetByToken(string token)
    {
        var user = await _user.SingleOrDefaultAsync(/*u => u.RefreshTokens.Any(t => t.Token == token)*/x => x.VerificationToken == token);
        
        if(user == null)
            throw new ArgumentException("Invalid token");
        return user;
    }

    public async Task<User> Create(User user)
    {
        if (_user.Any(u => u.Email == user.Email))
            throw new ArgumentException($"Email '{user.Email}' is already registered");

        await _context.AddAsync(user);
        _context.SaveChanges();
        //await _context.SaveChangesAsync();
        return user;
    }

    public async Task Delete(string id)
    {
        var user = await GetById(id);
        _user.Remove(user);
        await _context.SaveChangesAsync();
    }
}