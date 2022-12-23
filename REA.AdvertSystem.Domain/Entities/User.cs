using MongoDB.Bson.Serialization.Attributes;

namespace REA.AdvertSystem.Domain.Entities
{
    public class User
    {
        [BsonId]
        public string Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string? Patronymic { get; set; }
        
        public byte[]? Photo { get; set; }
    }
}
