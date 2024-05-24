using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.DTOs.Response;

namespace REA.AdvertSystem.Interfaces.Services;

public interface ISaveListService
{
    Task<PaginationResponse<AdvertsListResponse>> GetUserSaveList(UserSaveListRequest request);
    Task AddSaveListAsync(SaveListRequest request);
    Task DeleteSaveListAsync(string userId, string advertId);
}