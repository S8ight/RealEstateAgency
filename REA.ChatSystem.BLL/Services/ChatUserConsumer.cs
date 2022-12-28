using AuthorizationService.BLL.DTO.Request;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.BLL.Services;

public class ChatUserConsumer : IConsumer<QueueRequest>
{
    private IUserRepository _userRepository;
    private IMapper _mapper;
    private readonly ILogger<ChatUserConsumer> _logger;
    
    public ChatUserConsumer(IUserRepository userRepository, IMapper mapper, ILogger<ChatUserConsumer> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<QueueRequest> context)
    {
        try
        {
            var data = context.Message;

            var user = _mapper.Map<User>(data);
        
            await _userRepository.AddAsync(user);
            
            _logger.LogInformation("Added new user");   
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }

    }
}