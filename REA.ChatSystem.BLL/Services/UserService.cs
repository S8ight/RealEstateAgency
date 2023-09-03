using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
                var userResponse =  _mapper.Map<User, UserResponse>(user);
                
                if (user.Photo != null)
                {
                    userResponse.Photo = new FileContentResult(user.Photo, "image/jpeg");   
                }

                return userResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while receiving user: {Id}", id);
                throw;
            }
        }

        public async Task<string> AddAsync(UserRequest request)
        {
            try
            {
                var user = _mapper.Map<UserRequest, User>(request);
                var newUserId = await _unitOfWork.UserRepository.AddAsync(user);
                _unitOfWork.Commit();
                
                return newUserId;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating user");
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
                _logger.LogError(e, "Error occurred while deleting user: {Id}", id);
                throw;
            }
        }

        public async Task ReplaceAsync(UserRequest request)
        {
            try
            {
                var model = _mapper.Map<UserRequest, User>(request);
                await _unitOfWork.UserRepository.ReplaceAsync(model);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while updating user: {Id}", request.Id);
                throw;
            }
        }
    }
}
