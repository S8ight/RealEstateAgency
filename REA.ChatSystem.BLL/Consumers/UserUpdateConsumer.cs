using ApiGateWay.Models;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.BLL.Consumers;

public class UserUpdateConsumer : IConsumer<UserUpdateQueueModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserUpdateConsumer> _logger;
    
    public UserUpdateConsumer(IUserRepository userRepository, IMapper mapper, ILogger<UserUpdateConsumer> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserUpdateQueueModel> context)
    {
        try
        {
            var data = context.Message;

            var user = _mapper.Map<User>(data);
        
            await _userRepository.ReplaceAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the message from the UserUpdateQueue: {@Data}", context.Message);
            throw;
        }

    }
}