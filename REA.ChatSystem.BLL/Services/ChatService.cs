using AutoMapper;
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

        public ChatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ChatResponse>> GetAllAsync()
        {
            try
            {
                var chats = await _unitOfWork.ChatRepository.GetAllAsync();
                return chats?.Select(_mapper.Map<Chat, ChatResponse>);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<ChatResponse> GetByIdAsync(string id)
        {
            try
            {
                if (id == string.Empty) throw new ArgumentException("ChatId could not be empty");
            
                var chat = await _unitOfWork.ChatRepository.GetAsync(id);
                if (chat == null)
                {
                    throw new ArgumentException("Chat with that Id was not found");
                }
                var result = _mapper.Map<Chat, ChatResponse>(chat);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string> AddAsync(ChatRequest request)
        {
            var chat = _mapper.Map<ChatRequest, Chat>(request);
            var newChat = await _unitOfWork.ChatRepository.AddAsync(chat);
            if (newChat == String.Empty)
            {
                throw new ArgumentException("Not possible to create chat with this users");
            }
            _unitOfWork.Commit();
            return newChat;
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
                Console.WriteLine(e);
                throw;
            }
        }
    }

}
