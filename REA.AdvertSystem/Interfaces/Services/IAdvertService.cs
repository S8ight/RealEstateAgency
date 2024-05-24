using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.DTOs.Response;

namespace REA.AdvertSystem.Interfaces.Services;

public interface IAdvertService
{
    Task<PaginationResponse<AdvertsListResponse>> GetFilteredAdvertsAsync(AdvertsFilterRequest request);
    Task<PaginationResponse<AdvertsListResponse>> GetUserAdvertsAsync(UserSaveListRequest request);
    Task<PaginationResponse<AdvertsListResponse>> SearchAdvertsAsync(SearchAdvertsRequest request);
    Task<AdvertResponse> GetAdvertByIdAsync(string advertId);
    Task AddAdvertAsync(AdvertRequest request);
    Task UpdateAdvertAsync(string id, AdvertRequest request);
    Task DeleteAdvertAsync(string id);
}