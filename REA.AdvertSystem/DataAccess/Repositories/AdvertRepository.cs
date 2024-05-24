using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using REA.AdvertSystem.DataAccess.Entities;
using REA.AdvertSystem.Interfaces.Repositories;

namespace REA.AdvertSystem.DataAccess.Repositories;

public class AdvertRepository : IAdvertRepository
{
    private readonly AdvertDbContext _context;
    
    public AdvertRepository(AdvertDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<Advert> GetAdverts()
    {
        return _context.Adverts
            .Include(a => a.User)
            .Include(a => a.PhotoList
                .OrderBy(i => i.Id))
            .AsQueryable();
    }
    
    public IQueryable<Advert> GetUserAdverts(string userId)
    {
        return _context.Adverts
            .Include(a => a.PhotoList
                .OrderBy(i => i.Id))
            .Where(a => a.UserId == userId)
            .AsQueryable();
    }
    
    public Task<Advert> GetAvertByIdAsync(string id)
    {
        return _context.Adverts
            .Include(a => a.User)
            .Include(a => a.PhotoList
                .OrderBy(i => i.Id))
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    
    public async Task AddAdvertAsync(Advert advert)
    {
        await _context.Adverts.AddAsync(advert);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAdvertAsync(Advert advert)
    {
        _context.Adverts.Update(advert);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAdvertAsync(string id)
    {
        var advert = await _context.Adverts.FindAsync(id);
        
        if (advert == null)
        {
            throw new ArgumentNullException($"Advert with {id} not found");
        }
        
        _context.Adverts.Remove(advert);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Advert>> ListAsync(ISpecification<Advert> spec)
    {
        var query = SpecificationEvaluator.Default.GetQuery(GetAdverts(), spec);
        return await query.ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<Advert> spec)
    {
        var query = SpecificationEvaluator.Default.GetQuery(GetAdverts(), spec);
        return await query.CountAsync();
    }
}