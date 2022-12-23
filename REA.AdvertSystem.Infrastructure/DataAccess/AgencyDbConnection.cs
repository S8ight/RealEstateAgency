using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;

namespace REA.AdvertSystem.Infrastructure.DataAccess
{
    public class AgencyDbConnection : IAgencyDbConnection
    {
        private const string ConnectionString = "mongodb://root:mongoDB123@localhost:1338";
        private const string DatabaseName = "AgencyAdvertSystem";
        private const string AdvertCollection = "Advert";
        private const string PhotoListCollection = "PhotoList";
        private const string SaveListCollection = "SaveList";
        private const string UserCollection = "Users";

        public IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<T>(collection);
        }
    }
}
