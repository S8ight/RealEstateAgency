using MongoDB.Bson.Serialization.Attributes;

namespace REA.AdvertSystem.Domain.Entities
{
    public class PhotoList
    {
        [BsonId]
        public string Id { get; set; }

        public string AdvertId { get; set;}

        public string PhotoLink { get; set; }
    }
}
