using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;

namespace REA.AdvertSystem.Infrastructure.DataAccess
{
    public class AgencyDbConnection : IAgencyDbConnection
    {
        private readonly IConfiguration _configuration;

        public AgencyDbConnection(IConfiguration configuration)
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
}
