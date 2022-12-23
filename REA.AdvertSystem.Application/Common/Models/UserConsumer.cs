using AutoMapper;
using MassTransit;
using MediatR;
using REA.AdvertSystem.Application.Common.DTO.UserDTO;
using REA.AdvertSystem.Application.Users.Commands;

namespace REA.AdvertSystem.Application.Common.Models;

public class UserConsumer : IConsumer<QueueRequest>
{
    private IMediator _mediator = null!;

    private readonly IMapper _mapper;
    protected IMediator Mediator => _mediator;

    public UserConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<QueueRequest> context)
    {
        var data = context.Message;

        var user = _mapper.Map<CreateUserCommand>(data);
        
        await Mediator.Send(user);
    }
}