using DiscountService.DataAccess.Interfaces;
using MongoDB.Driver;

namespace DiscountService.DataAccess;

public class DiscountDbContext : IDiscountDbContext
{
    private readonly IConfiguration _configuration;

    public DiscountDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IMongoCollection<T> ConnectToMongo<T>(in string collection)
    {
        var client = new MongoClient(_configuration["AdvertDatabase:ConnectionString"]);
        var db = client.GetDatabase(_configuration["AdvertDatabase:DatabaseName"]);
        return db.GetCollection<T>(collection);
    }
}