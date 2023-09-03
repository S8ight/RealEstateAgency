using ApiGateWay.Models;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.BLL.Consumers;

public class UserRegistrationConsumer : IConsumer<UserRegistrationQueueModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserRegistrationConsumer> _logger;
    
    public UserRegistrationConsumer(IUserRepository userRepository, IMapper mapper, ILogger<UserRegistrationConsumer> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserRegistrationQueueModel> context)
    {
        try
        {
            var data = context.Message;

            var user = _mapper.Map<User>(data);
        
            await _userRepository.AddAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the message from the UserRegistrationQueue: {@Data}", context.Message);
            throw;
        }

    }
}