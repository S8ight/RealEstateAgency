namespace DiscountService.DTO;

public class DiscountRequest
{
    public string AdvertId { get; set; }
    public int Percentage { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime ExpireAt { get; set; }
}