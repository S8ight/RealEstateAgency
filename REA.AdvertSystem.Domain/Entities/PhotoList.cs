using MongoDB.Bson.Serialization.Attributes;

namespace REA.AdvertSystem.Domain.Entities
{
    public class PhotoList
    {
        [BsonId]
        public string PhotoID { get; set; }

        public string AdvertID { get; set;}

        public string PhotoLink { get; set; }
    }
}
