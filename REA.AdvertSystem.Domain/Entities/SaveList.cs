using MongoDB.Bson.Serialization.Attributes;

namespace REA.AdvertSystem.Domain.Entities
{
    public class SaveList
    {
        [BsonId]
        public string ListID { get; set; }

        public string AdvertID { get; set; }

        public string UserID { get; set; }
    }
}
