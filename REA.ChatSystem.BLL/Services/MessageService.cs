using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;
using REA.ChatSystem.BLL.Hubs;
using REA.ChatSystem.BLL.Interfaces;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<ChatHub> hubContext, ILogger<MessageService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<IEnumerable<MessageResponse>> GetAllMessagesForChatAsync(string chatId)
        {
            try
            {
                var messages = await _unitOfWork.MessageRepository.GetMessagesOfChat(chatId);
                var messagesResponse = messages.Select(_mapper.Map<Message, MessageResponse>);

                return messagesResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while receiving chat({Id}) messages", chatId);
                throw;
            }
        }

        public async Task<MessageResponse> GetAsync(string id)
        {
            try
            {
                var message = await _unitOfWork.MessageRepository.GetAsync(id);
                var messageResponse = _mapper.Map<Message, MessageResponse>(message);

                return messageResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while receiving chatMessage: {Id}", id);
                throw;
            }
        }

        public async Task UpdateAsync(MessageUpdateRequest request)
        {
            try
            {
                var message = await _unitOfWork.MessageRepository.GetAsync(request.MessageId);
                message.MessageBody = request.MessageBody;
                
                await _unitOfWork.MessageRepository.ReplaceAsync(message);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while updating chatMessage: {Id}", request.MessageId);
                throw;
            }

        }

        public async Task<string> AddAsync(MessageRequest request)
        {
            try
            {
                var message = _mapper.Map<MessageRequest, Message>(request);
                
                var messageId = await _unitOfWork.MessageRepository.AddAsync(message);
                _unitOfWork.Commit();

                return messageId;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating chatMessage");
                throw;
            }
        }

        public async Task DeleteAsync(MessageDeleteRequest request)
        {
            try
            {
                await _unitOfWork.MessageRepository.DeleteAsync(request.MessageId);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while deleting chatMessage: {Id}", request.MessageId);
                throw;
            }
        }
    }
}
