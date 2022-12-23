using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;

namespace REA.ChatSystem.BLL.Interfaces
{
    public interface IChatService
    {
        public Task<IEnumerable<ChatResponse>> GetAllAsync();
        public Task<ChatResponse> GetByIdAsync(string id);
        public Task<string> AddAsync(ChatRequest request);
        public Task DeleteAsync(string id);
    }
}
