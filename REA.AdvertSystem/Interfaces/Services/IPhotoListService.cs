using REA.AdvertSystem.DTOs.Request;

namespace REA.AdvertSystem.Interfaces.Services;

public interface IPhotoListService
{
    Task UploadImagesAsync(UploadPhotoListRequest request);
    Task DeletePhotoAsync(string id);
    
}