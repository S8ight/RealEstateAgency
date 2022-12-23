using DiscountGrpcService.Extensions;
using DiscountGrpcService.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace DiscountGrpcService.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IDistributedCache _redisCache;
    
    public DiscountRepository(IDistributedCache cache)
    {
        _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
    }
    
    public async Task<Discount.Discount?> GetDiscount(string id)
    {
        return await _redisCache.GetRecordAsync<Discount.Discount>(id);
    }

    public async Task AddDiscount(Discount.Discount discount)
    {
        await _redisCache.SetRecordAsync(discount.AdvertId, discount, discount.ExpieredAt.TimeOfDay, discount.ExpieredAt.TimeOfDay);
    }

    public async Task DeleteDiscount(string id)
    {
        await _redisCache.RemoveAsync(id);
    }
    
}