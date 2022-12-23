using AutoMapper;
using MassTransit;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.BLL.Services;

public class UserConsumer : IConsumer<QueueRequest>
{
    private IUserRepository _userRepository;
    private IMapper _mapper;
    
    public UserConsumer(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<QueueRequest> context)
    {
        var data = context.Message;

        var user = _mapper.Map<User>(data);
        
        await _userRepository.AddAsync(user);

    }
}