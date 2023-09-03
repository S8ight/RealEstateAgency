namespace REA.AdvertSystem.Application.Common.DTO.AdvertDTO
{
    public class AdvertRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Adress { get; set; }
        
        public string Country { get; set; }
        
        public string Settlement { get; set; }

        public float Square { get; set; }

        public float Price { get; set; }

        public DateTime Created { get; set; }

        public string UserId { get; set; }

        public string EstateType { get; set; }

        public bool IsForSale { get; set; }

        public bool IsForRent { get; set; }
    }
}
