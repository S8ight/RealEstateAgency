namespace REA.AdvertSystem.DTOs.Request;

public class UploadPhotoListRequest
{
    public string AdvertId { get; set; }
    public List<IFormFile> PhotoList { get; set; }
}