using AutoMapper;
using Microsoft.Extensions.Logging;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;
using REA.ChatSystem.BLL.Interfaces;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.BLL.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ChatService> _logger;

        public ChatService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ChatService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<ChatResponse>> GetAllAsync()
        {
            try
            {
                var chats = await _unitOfWork.ChatRepository.GetAllAsync();
                var chatsResponse = chats.Select(_mapper.Map<Chat, ChatResponse>);

                return chatsResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while receiving chats");
                throw;
            }
        }
        
        public async Task<IEnumerable<ChatResponse>?> GetUserChats(string userId)
        {
            try
            {
                var chats = await _unitOfWork.ChatRepository.GetUserChats(userId);
                var chatsResponse = chats.Select(_mapper.Map<Chat, ChatResponse>);
                
                return chatsResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while receiving user chats");
                throw;
            }
        }
        
        public async Task<ChatResponse> GetByIdAsync(string id)
        {
            try
            {
                if (id == string.Empty)
                {
                    throw new ArgumentException("ChatId could not be empty");
                }
                var chat = await _unitOfWork.ChatRepository.GetAsync(id);
                
                if (chat == null)
                {
                    throw new ArgumentException("Chat was not found");
                }
                var chatResponse = _mapper.Map<Chat, ChatResponse>(chat);
                
                return chatResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while receiving chat: {Id}", id);
                throw;
            }
        }

        public async Task<string> AddAsync(ChatRequest request)
        {
            try
            {
                var chat = _mapper.Map<ChatRequest, Chat>(request);
                var chatId = await _unitOfWork.ChatRepository.AddAsync(chat);
                
                if (chatId == String.Empty)
                {
                    throw new ArgumentException("Not possible to create a chat with these users");
                }
                _unitOfWork.Commit();
                
                return chatId;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating chat");
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                await _unitOfWork.ChatRepository.DeleteAsync(id);
                
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while deleting chat");
                throw;
            }
        }
    }

}
