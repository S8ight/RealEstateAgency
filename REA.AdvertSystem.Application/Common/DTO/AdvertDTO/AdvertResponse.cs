using MassTransit.Futures.Contracts;
using REA.AdvertSystem.Application.Common.DTO.PhotoListDTO;
using REA.AdvertSystem.Application.Common.DTO.UserDTO;

namespace REA.AdvertSystem.Application.Common.DTO.AdvertDTO
{
    public class AdvertResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Adress { get; set; }
        
        public string Country { get; set; }
        
        public string Settlement { get; set; }
        
        public string UserId { get; set; }

        public float Square { get; set; }

        public float Price { get; set; }
        public float PriceWithDiscount { get; set; }

        public DateTime Created { get; set; }
        public DateTime DiscountExpirationTime { get; set; }

        public string EstateType { get; set; }

        public bool IsForSale { get; set; }

        public bool IsForRent { get; set; }
        public IEnumerable<PhotoResponse> PhotoList { get; set; }
        
        public UserResponse Seller { get; set; }
        
    }
}
