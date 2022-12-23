namespace REA.AdvertSystem.Application.Common.DTO.AdvertDTO
{
    public class AdvertRequest
    {
        public string AdvertID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Town { get; set; }

        public string Adress { get; set; }

        public string Coordinates { get; set; }

        public float Square { get; set; }

        public float Price { get; set; }

        public DateTime Created { get; set; }

        public int SellerID { get; set; }

        public bool Flat { get; set; }

        public bool Sell { get; set; }

        public bool Rent { get; set; }
    }
}
