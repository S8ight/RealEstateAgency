using ApiGateWay.Models;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using REA.AdvertSystem.Application.Users.Commands;

namespace REA.AdvertSystem.Application.Common.Consumers;

public class UserDeleteConsumer : IConsumer<UserDeleteQueueModel>
{
    private IMediator _mediator;
    private IMediator Mediator => _mediator;

    private readonly IMapper _mapper;
    
    private readonly ILogger<UserDeleteConsumer> _logger;

    public UserDeleteConsumer(IMapper mapper, IMediator mediator, ILogger<UserDeleteConsumer> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<UserDeleteQueueModel> context)
    {
        try
        {
            var data = context.Message;

            var user = _mapper.Map<DeleteUserCommand>(data);
        
            await Mediator.Send(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the message from the UserDeleteQueue: {@Data}", context.Message);
            throw;
        }
    }
}