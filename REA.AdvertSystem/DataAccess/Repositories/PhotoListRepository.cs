using REA.AdvertSystem.DataAccess.Entities;
using REA.AdvertSystem.Interfaces.Repositories;

namespace REA.AdvertSystem.DataAccess.Repositories;

public class PhotoListRepository : IPhotoListRepository
{
    private readonly AdvertDbContext _context;
    
    public PhotoListRepository(AdvertDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<PhotoList> GetAdvertPhotoList(string advertId)
    {
        return _context.PhotoLists
            .Where(p => p.AdvertId == advertId)
            .AsQueryable();
    }
    
    public async Task<PhotoList?> GetPhotoListByIdAsync(string id)
    {
        return await _context.PhotoLists.FindAsync(id);
    }
    
    public async Task AddPhotoListAsync(PhotoList photoList)
    {
        await _context.PhotoLists.AddAsync(photoList);
        await _context.SaveChangesAsync();
    }
    
    public async Task AddRangeOfPhotoListsAsync(List<PhotoList> photoLists)
    {
        await _context.PhotoLists.AddRangeAsync(photoLists);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeletePhotoListAsync(string id)
    {
        var photoList = await _context.PhotoLists.FindAsync(id);
        
        if (photoList == null)
        {
            throw new ArgumentNullException($"PhotoList with {id} not found");
        }
        
        _context.PhotoLists.Remove(photoList);
        await _context.SaveChangesAsync();
    }
}