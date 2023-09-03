using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;

namespace REA.ChatSystem.BLL.Interfaces
{
    public interface IMessageService
    {
        public Task<MessageResponse> GetAsync(string id);
        public Task<IEnumerable<MessageResponse>> GetAllMessagesForChatAsync(string chatId);
        public Task UpdateAsync(MessageUpdateRequest request);
        public Task<string> AddAsync(MessageRequest request);
        public Task DeleteAsync(MessageDeleteRequest request);
    }
}
