using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.DTOs.Response;

namespace REA.AdvertSystem.Interfaces.Services;

public interface IUserService
{
    Task<UserResponse> GetUserByIdAsync(string userId);
    Task AddUserAsync(UserRequest request);
    Task DeleteUser(string userId);
}