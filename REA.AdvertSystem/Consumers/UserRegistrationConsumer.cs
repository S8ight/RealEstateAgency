using ApiGateWay.Models;
using AutoMapper;
using MassTransit;
using MediatR;
using REA.AdvertSystem.Application.Users.Commands;
using REA.AdvertSystem.DataAccess.Entities;
using REA.AdvertSystem.Interfaces.Repositories;

namespace REA.AdvertSystem.Consumers;

public class UserRegistrationConsumer : IConsumer<UserRegistrationQueueModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserRegistrationConsumer> _logger;

    public UserRegistrationConsumer(IMapper mapper, ILogger<UserRegistrationConsumer> logger, IUserRepository userRepository)
    {
        _mapper = mapper;
        _logger = logger;
        _userRepository = userRepository;
    }
    public async Task Consume(ConsumeContext<UserRegistrationQueueModel> context)
    {
        try
        {
            var data = context.Message;

            var user = _mapper.Map<User>(data);

            await _userRepository.AddUserAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the message from the UserRegistrationQueue: {@Data}", context.Message);
            throw;
        }
    }
}