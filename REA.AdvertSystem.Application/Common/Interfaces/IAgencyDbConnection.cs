using MongoDB.Driver;

namespace REA.AdvertSystem.Application.Common.Interfaces
{
    public interface IAgencyDbConnection
    {
        public IMongoCollection<T> ConnectToMongo<T>(in string collection);

        //Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
