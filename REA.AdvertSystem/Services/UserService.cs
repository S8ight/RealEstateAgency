using AutoMapper;
using REA.AdvertSystem.DataAccess.Entities;
using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.DTOs.Response;
using REA.AdvertSystem.Interfaces.Repositories;
using REA.AdvertSystem.Interfaces.Services;

namespace REA.AdvertSystem.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UserResponse> GetUserByIdAsync(string userId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);

        return _mapper.Map<UserResponse>(user);
    }

    public async Task AddUserAsync(UserRequest request)
    {
        var user = _mapper.Map<User>(request);
        await _unitOfWork.UserRepository.AddUserAsync(user);
    }

    public async Task DeleteUser(string userId)
    {
        await _unitOfWork.UserRepository.DeleteUserAsync(userId);
    }
}