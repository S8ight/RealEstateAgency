using ApiGateWay.Models;
using AutoMapper;
using MassTransit;
using MediatR;
using REA.AdvertSystem.Application.Users.Commands;
using REA.AdvertSystem.Interfaces.Repositories;

namespace REA.AdvertSystem.Consumers;

public class UserDeleteConsumer : IConsumer<UserDeleteQueueModel>
{

    private readonly IUserRepository _userRepository;
    
    private readonly ILogger<UserDeleteConsumer> _logger;

    public UserDeleteConsumer(ILogger<UserDeleteConsumer> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }
    public async Task Consume(ConsumeContext<UserDeleteQueueModel> context)
    {
        try
        {
            var data = context.Message;
        
            await _userRepository.DeleteUserAsync(data.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the message from the UserDeleteQueue: {@Data}", context.Message);
            throw;
        }
    }
}