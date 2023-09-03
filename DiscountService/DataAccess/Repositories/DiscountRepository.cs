using DiscountService.DataAccess.Interfaces;
using DiscountService.Entities;
using MongoDB.Driver;

namespace DiscountService.DataAccess.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private IMongoCollection<Discount> DiscountCollection { get; }

    private readonly ILogger<DiscountRepository> _logger;
    
    public DiscountRepository( IDiscountDbContext context, ILogger<DiscountRepository> logger)
    {
        DiscountCollection = context.ConnectToMongo<Discount>("Discounts");
        _logger = logger;
    }
    
    public async Task<Discount?> GetAdvertCurrentDiscount(string advertId)
    {
        try
        {
            DateTime currentDate = DateTime.UtcNow;

            var filter = Builders<Discount>.Filter.And(
                Builders<Discount>.Filter.Eq("AdvertId", advertId),
                Builders<Discount>.Filter.Lte("StartAt", currentDate),
                Builders<Discount>.Filter.Gte("ExpireAt", currentDate) 
            );

            var discount = await DiscountCollection.Find(filter).FirstOrDefaultAsync();
            
            return discount;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while receiving the discount");
            throw;
        }
    }
    
    public async Task<IEnumerable<Discount>> GetAdvertDiscounts(string advertId)
    {
        try
        {
            var discountList = await DiscountCollection.Find(d => d.AdvertId == advertId).ToListAsync();
            
            return discountList;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while receiving discounts for advert({AdvertId})", advertId);
            throw;
        }
    }

    public async Task<Discount> GetDiscountById(string id)
    {
        try
        {
            var discount = await DiscountCollection.Find(d => d.Id == id).FirstOrDefaultAsync()
                           ?? throw new KeyNotFoundException($"Discount with id({id}) could not be found");
                               
            return discount;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while receiving discount");
            throw;
        }
    }

    public async Task AddDiscount(Discount discount)
    {
        try
        {
            var filter = Builders<Discount>.Filter.Or(
                Builders<Discount>.Filter.And(
                    Builders<Discount>.Filter.Eq("AdvertId", discount.AdvertId),
                    Builders<Discount>.Filter.Gte("StartAt", discount.StartAt),
                    Builders<Discount>.Filter.Lte("StartAt", discount.ExpireAt)
                ),
                Builders<Discount>.Filter.And(
                    Builders<Discount>.Filter.Eq("AdvertId", discount.AdvertId),
                    Builders<Discount>.Filter.Gte("ExpireAt", discount.StartAt),
                    Builders<Discount>.Filter.Lte("ExpireAt", discount.ExpireAt)
                )
            );

            var discountExist = await DiscountCollection.Find(filter).FirstOrDefaultAsync();
            
            if (discountExist != null)
            {
                throw new ArgumentException($"Discount for advert({discount.AdvertId}) for that time already exist");
            }

            discount.Id = Guid.NewGuid().ToString();
            discount.Created = DateTime.UtcNow;
            await DiscountCollection.InsertOneAsync(discount);
            _logger.LogInformation("Added new discount({DiscountId}) for advert({AdvertId})", discount.Id, discount.AdvertId);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error occurred while adding new discount");
            throw;
        }
    }

    public async Task DeleteAdvertDiscounts(string advertId)
    {
        try
        {
            await DiscountCollection.DeleteManyAsync(d => d.AdvertId == advertId);
            _logger.LogInformation("Deleted all advert({AdvertId}) discounts",advertId);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error occurred while deleting discounts for advert({AdvertId})", advertId);
            throw;
        }
    }
    
    public async Task DeleteDiscount(string id)
    {
        try
        {
            await DiscountCollection.DeleteOneAsync(d => d.Id == id);
            _logger.LogInformation("Deleted discount({DiscountId})", id);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error occurred while deleting discount({DiscountId})", id);
            throw;
        }
    }
    
}