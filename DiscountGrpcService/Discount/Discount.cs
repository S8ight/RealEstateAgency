namespace DiscountGrpcService.Discount;

public class Discount
{
    public int Percentage { get; set; }
    public DateTime ExpieredAt { get; set; }
    public DateTime Created { get; set; }
    public string AdvertId { get; set; }
}