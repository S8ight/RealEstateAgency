namespace DiscountService.DTO;

public class DiscountRequest
{
    public int Percentage { get; set; }
    public DateTime ExpieredAt { get; set; }
    public string AdvertId { get; set; }
}