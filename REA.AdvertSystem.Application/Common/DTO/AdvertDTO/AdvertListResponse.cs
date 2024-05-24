namespace REA.AdvertSystem.Application.Common.DTO.AdvertDTO;

public class AdvertListResponse
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Country { get; set; }
        
    public string Settlement { get; set; }

    public string Address { get; set; }

    public float Price { get; set; }
    
    public float? PriceWithDiscount { get; set; }
    
    public DateTime? DiscountExpirationTime { get; set; }

    public DateTime Created { get; set; }

    public string PhotoUrl { get; set; }
}