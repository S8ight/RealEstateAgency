using MongoDB.Bson.Serialization.Attributes;

namespace REA.AdvertSystem.Domain.Entities
{
    public class Advert
    {
        [BsonId]
        public string AdvertID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Adress { get; set; }

        public float Square { get; set; }

        public float Price { get; set; }

        public DateTime Created { get; set; }

        public string UserID { get; set; }

        public string AdvertType { get; set; }
    }
}
