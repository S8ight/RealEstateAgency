using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;

namespace REA.ChatSystem.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> GetAsync(string id);
        public Task<string> AddAsync(UserRequest request);
        public Task DeleteAsync(string id);
        public Task ReplaceAsync(UserRequest request);

    }
}
