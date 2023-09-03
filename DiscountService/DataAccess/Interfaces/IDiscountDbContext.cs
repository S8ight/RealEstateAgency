using MongoDB.Driver;

namespace DiscountService.DataAccess.Interfaces;

public interface IDiscountDbContext
{
    public IMongoCollection<T> ConnectToMongo<T>(in string collection);
}