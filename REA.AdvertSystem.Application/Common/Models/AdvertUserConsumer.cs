using AuthorizationService.BLL.DTO.Request;
using AutoMapper;
using MassTransit;
using MediatR;
using REA.AdvertSystem.Application.Users.Commands;

namespace REA.AdvertSystem.Application.Common.Models;

public class AdvertUserConsumer : IConsumer<QueueRequest>
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator;

    private readonly IMapper _mapper;

    public AdvertUserConsumer(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<QueueRequest> context)
    {
        var data = context.Message;

        var user = _mapper.Map<CreateUserCommand>(data);
        
        await Mediator.Send(user);
    }
}