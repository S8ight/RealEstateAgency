﻿using MongoDB.Bson.Serialization.Attributes;

namespace REA.AdvertSystem.Domain.Entities
{
    public class SaveList
    {
        [BsonId]
        public string Id { get; set; }

        public string AdvertId { get; set; }

        public string UserId { get; set; }
    }
}
