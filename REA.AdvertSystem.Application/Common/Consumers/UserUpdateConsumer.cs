using ApiGateWay.Models;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using REA.AdvertSystem.Application.Users.Commands;

namespace REA.AdvertSystem.Application.Common.Consumers;

public class UserUpdateConsumer : IConsumer<UserUpdateQueueModel>
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator;

    private readonly IMapper _mapper;
    
    private readonly ILogger<UserUpdateConsumer> _logger;

    public UserUpdateConsumer(IMapper mapper, IMediator mediator, ILogger<UserUpdateConsumer> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<UserUpdateQueueModel> context)
    {
        try
        {
            var data = context.Message;

            var user = _mapper.Map<UpdateUserCommand>(data);
        
            await Mediator.Send(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while processing the message from the UserUpdateQueue: {@Data}", context.Message);
            throw;
        }
    }
}