using AutoMapper;
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

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserResponse> GetAsync(string id)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(id);
            return _mapper.Map<User, UserResponse>(user);
        }

        public async Task<string> AddAsync(UserRequest request)
        {
            var model = _mapper.Map<UserRequest, User>(request);
            var newId = await _unitOfWork.UserRepository.AddAsync(model);
            _unitOfWork.Commit();
            return newId;
        }

        public async Task DeleteAsync(string id)
        {
            await _unitOfWork.UserRepository.DeleteAsync(id);
            _unitOfWork.Commit();
        }

        public async Task ReplaceAsync(UserRequest request)
        {
            var model = _mapper.Map<UserRequest, User>(request);
            var newId = await _unitOfWork.UserRepository.ReplaceAsync(model);
            _unitOfWork.Commit();
        }
    }
}
