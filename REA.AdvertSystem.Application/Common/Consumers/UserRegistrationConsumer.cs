using ApiGateWay.Models;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using REA.AdvertSystem.Application.Users.Commands;

namespace REA.AdvertSystem.Application.Common.Consumers;

public class UserRegistrationConsumer : IConsumer<UserRegistrationQueueModel>
{
    private IMediator _mediator;
    private IMediator Mediator => _mediator;

    private readonly IMapper _mapper;
    
    private readonly ILogger<UserRegistrationConsumer> _logger;

    public UserRegistrationConsumer(IMapper mapper, IMediator mediator, ILogger<UserRegistrationConsumer> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<UserRegistrationQueueModel> context)
    {
        try
        {
            var data = context.Message;

            var user = _mapper.Map<CreateUserCommand>(data);
        
            await Mediator.Send(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the message from the UserRegistrationQueue: {@Data}", context.Message);
            throw;
        }
    }
}