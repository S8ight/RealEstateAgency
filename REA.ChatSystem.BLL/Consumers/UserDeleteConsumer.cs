using ApiGateWay.Models;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.BLL.Consumers;

public class UserDeleteConsumer : IConsumer<UserDeleteQueueModel>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserDeleteConsumer> _logger;
    
    public UserDeleteConsumer(IUserRepository userRepository, ILogger<UserDeleteConsumer> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserDeleteQueueModel> context)
    {
        try
        {
            var data = context.Message;

            await _userRepository.DeleteAsync(data.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the message from the UserDeleteQueue: {@Data}", context.Message);
            throw;
        }

    }
}