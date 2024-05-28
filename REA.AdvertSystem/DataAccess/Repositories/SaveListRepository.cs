using Microsoft.EntityFrameworkCore;
using REA.AdvertSystem.DataAccess.Entities;
using REA.AdvertSystem.Interfaces.Repositories;

namespace REA.AdvertSystem.DataAccess.Repositories;

public class SaveListRepository : ISaveListRepository
{
    private readonly AdvertDbContext _context;
    
    public SaveListRepository(AdvertDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<SaveList> GetUserSaveList(string userId)
    {
        return _context.SaveLists
            .Include(sl => sl.Advert)
            .Include(a => a.Advert.PhotoList)
            .Where(p => p.UserId == userId)
            .AsQueryable();
    }
    
    public async Task AddSaveListAsync(SaveList saveList)
    {
        await _context.SaveLists.AddAsync(saveList);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteSaveListAsync(string userId, string advertId)
    {
        var saveList = await _context.SaveLists.FirstOrDefaultAsync(sl => sl.UserId == userId && sl.AdvertId == advertId);
        
        if (saveList == null)
        {
            throw new ArgumentNullException($"SaveList not found");
        }
        
        _context.SaveLists.Remove(saveList);
        await _context.SaveChangesAsync();
    }
}