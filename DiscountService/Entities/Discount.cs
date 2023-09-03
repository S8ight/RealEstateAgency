using MongoDB.Bson.Serialization.Attributes;

namespace DiscountService.Entities;

public class Discount
{
    [BsonId]
    public string Id { get; set; }
    public string AdvertId { get; set; }
    public int Percentage { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public DateTime Created { get; set; }
}