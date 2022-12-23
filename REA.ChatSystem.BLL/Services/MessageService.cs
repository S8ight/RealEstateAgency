using AutoMapper;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;
using REA.ChatSystem.BLL.Interfaces;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MessageResponse>> GetAllAsync()
        {
            var message = await _unitOfWork.MessageRepository.GetAllAsync();
            return message?.Select(_mapper.Map<Message, MessageResponse>);
        }
        
        public async Task<IEnumerable<MessageResponse>> GetAllMessagesForChatAsync(string chatId)
        {
            var message = await _unitOfWork.MessageRepository.GetMessagesOfChat(chatId);
            return message?.Select(_mapper.Map<Message, MessageResponse>);
        }

        public async Task<MessageResponse> GetAsync(string id)
        {
            var message = await _unitOfWork.MessageRepository.GetAsync(id);
            return _mapper.Map<Message, MessageResponse>(message);
        }

        public async Task UpdateAsync(MessageRequest request)
        {
            var message = _mapper.Map<MessageRequest, Message>(request);
            await _unitOfWork.MessageRepository.ReplaceAsync(message);
            _unitOfWork.Commit();
        }

        public async Task<string> AddAsync(MessageRequest request)
        {
            var message = _mapper.Map<MessageRequest, Message>(request);
            var newId = await _unitOfWork.MessageRepository.AddAsync(message);
            _unitOfWork.Commit();
            return newId;
        }

        public async Task DeleteAsync(string id)
        {
            await _unitOfWork.MessageRepository.DeleteAsync(id);
            _unitOfWork.Commit();
        }
    }
}
