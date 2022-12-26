using AutoMapper;
using Microsoft.Extensions.Logging;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;
using REA.ChatSystem.BLL.Interfaces;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserResponse> GetAsync(string id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(id);
                return _mapper.Map<User, UserResponse>(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<string> AddAsync(UserRequest request)
        {
            try
            {
                var model = _mapper.Map<UserRequest, User>(request);
                var newId = await _unitOfWork.UserRepository.AddAsync(model);
                _unitOfWork.Commit();
                return newId;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                await _unitOfWork.UserRepository.DeleteAsync(id);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task ReplaceAsync(UserRequest request)
        {
            try
            {
                var model = _mapper.Map<UserRequest, User>(request);
                var newId = await _unitOfWork.UserRepository.ReplaceAsync(model);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}
