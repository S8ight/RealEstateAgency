
using DiscountService.Entities;
using DiscountService.Extensions;
using DiscountService.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace DiscountService.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IDistributedCache _redisCache;
    
    public DiscountRepository(IDistributedCache cache)
    {
        _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
    }
    
    public async Task<Discount?> GetDiscount(string id)
    {
        return await _redisCache.GetRecordAsync<Discount>(id);
    }

    public async Task AddDiscount(Discount discount)
    {
        await _redisCache.SetRecordAsync(discount.AdvertId, discount, discount.ExpieredAt.TimeOfDay, discount.ExpieredAt.TimeOfDay);
    }

    public async Task DeleteDiscount(string id)
    {
        await _redisCache.RemoveAsync(id);
    }
    
}