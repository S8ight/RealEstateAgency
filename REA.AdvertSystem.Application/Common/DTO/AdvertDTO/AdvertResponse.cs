namespace REA.AdvertSystem.Application.Common.DTO.AdvertDTO
{
    public class AdvertResponse
    {
        public string AdvertID { get; set; }

        public string Name { get; set; }

        public string Town { get; set; }

        public float Square { get; set; }

        public float Price { get; set; }

        public DateTime Created { get; set; }
    }
}
