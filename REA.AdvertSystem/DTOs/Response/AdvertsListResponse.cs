namespace REA.AdvertSystem.DTOs.Response;

public class AdvertsListResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Province { get; set; }
    public string Settlement { get; set; }
    public float Price { get; set; }
    public string EstateType { get; set; }
    public string PhotoPreview { get; set; }
}